# PROJECT.md — PCConfigurator

Данный репозиторий содержит учебный проект «PCConfigurator» — веб‑приложение для подбора и сборки конфигурации ПК. Проект решает задачу упрощения сборки компьютера для пользователя за счёт удобного каталога комплектующих, сохранения собранных конфигураций, роли администратора для наполнения базы комплектующих и вспомогательных функций (чат‑бот). Важная часть пользовательского опыта — автоматическая проверка совместимости: при выборе компонента несовместимые варианты в других категориях визуально помечаются (становятся «серыми») и не выбираются, а при наведении показывается причина несовместимости.

[main](screenshots/main.png)

## 1) Цель проекта

Цель проекта — предоставить пользователю понятный интерфейс для сборки ПК из набора комплектующих и помочь избежать типовых ошибок совместимости. Пользователь выбирает процессор, материнскую плату, оперативную память, видеокарту, накопители, блок питания, корпус и термопасту, после чего может сохранить сборку в личный список. Дополнительно реализованы:

- авторизация/регистрация с использованием JWT;
- роли пользователей (обычный пользователь и администратор);
- админ‑панель для добавления комплектующих в каталог;
- сохранение сборок и отображение их в разделе конфигураций;
- лайки сборок;
- чат‑бот (интеграция с сервисом GigaChat).

## 2) Технологии и стек

Backend реализован на ASP.NET Core (целевой фреймворк `net8.0`). Для хранения данных используется PostgreSQL. Доступ к БД реализован через Entity Framework Core (провайдер Npgsql) и миграции. Для управления пользователями и ролями используется ASP.NET Core Identity, а авторизация запросов к защищённым эндпоинтам — через JWT Bearer.

Frontend — SPA на React (Create React App / `react-scripts`) с маршрутизацией через `react-router-dom`. Взаимодействие с API выполняется через `axios`. Часть логики совместимости реализована на клиенте (в интерфейсе конфигуратора) и основана на данных, полученных с API.

Для удобного запуска проект контейнеризирован через Docker Compose:

- контейнер `postgres` — PostgreSQL;
- контейнер `api` — ASP.NET Core API;
- контейнер `client` — статическая сборка React, раздаваемая `nginx`.

Для корректной работы SPA‑маршрутов (например `/admin`) nginx настроен на fallback `try_files ... /index.html`, чтобы при прямом открытии роутов не возвращался 404.

## 3) Общая архитектура и взаимодействие компонентов

Приложение состоит из трёх основных слоёв: клиент (React), сервер (ASP.NET Core API) и база данных (PostgreSQL). Клиент запрашивает каталоги комплектующих через REST‑эндпоинты вида `GET /api/CPU`, `GET /api/Motherboard`, `GET /api/RAM` и т.д., отображает карточки и позволяет добавлять элементы в текущую сборку.

После выбора компонентов клиент выполняет локальную проверку совместимости: для каждой карточки вычисляется, совместима ли она с текущей сборкой. Если несовместима — карточка становится полупрозрачной и недоступной для выбора. При наведении отображается причина (или несколько причин), по которым элемент несовместим.

Сохранение сборки происходит через эндпоинт `POST /api/PCBuild/save`. Сервер принимает DTO с идентификаторами выбранных комплектующих, вычисляет итоговую стоимость и сохраняет запись сборки в БД, привязывая её к текущему пользователю (по `UserId` из JWT).

Раздел администрирования доступен по маршруту `/admin` (на клиенте) и ограничен по роли `Admin`. Токен и роль сохраняются в `localStorage` после входа. Админ‑панель даёт возможность добавлять новые записи комплектующих через соответствующие API‑контроллеры.

## 4) Аутентификация, роли и доступ

- Регистрация: `POST /api/Auth/register` создаёт пользователя и назначает роль `User`.
- Логин: `POST /api/Auth/login` возвращает JWT и список ролей пользователя.
- Защита API: контроллер `PCBuildController` помечен `[Authorize]`, поэтому требует токен.
- Инициализация ролей и администратора выполняется при старте приложения в `DbInitializer.SeedRolesAndAdminAsync`. По умолчанию создаётся администратор:

  Логин: `admin`

  Пароль: `Admin123#@!`

## 5) Основные пользовательские фичи

### 5.1 Конфигуратор (сборка ПК)

Пользователь выбирает компоненты по категориям. Состояние текущей сборки хранится на клиенте. Совместимость пересчитывается динамически, а несовместимые варианты становятся недоступными. При изменении выбора несовместимые уже выбранные элементы автоматически сбрасываются.

### 5.2 Совместимость (реализована на клиенте)

Проверки основаны на полях, которые реально присутствуют в моделях и JSON‑seed данных. Движок совместимости возвращает массив причин несовместимости. На текущем этапе проверяются следующие зависимости:

- CPU ↔ Motherboard: соответствие сокета (`Socket`).
- RAM ↔ Motherboard: тип памяти (`RAMType` / `Type`) и ограничение частоты (в текущих данных поле `Motherboard.MaxRAM` используется как максимальная частота).
- SSD (NVMe/M.2) ↔ Motherboard: требуется `HasM2 == true`.
- PSU ↔ (CPU + GPU): оценивается требуемая мощность как `CPU.TDP + GPU.PowerConsumption + 150`.

Проверка по корпусу отключена (по требованию), то есть корпус не влияет на доступность других компонентов.

### 5.3 Сохранение сборок

Сборка сохраняется в таблицу `PCBuilds`, хранит ссылки на выбранные комплектующие, итоговую стоимость, пользователя‑владельца и дату создания.

### 5.4 Лайки сборок

Система лайков реализована таблицей `PCBuildLikes`, которая связывает пользователя и сборку.

### 5.5 Админ‑панель

Админ‑панель позволяет добавлять комплектующие. Доступ ограничен ролью `Admin`.

### 5.6 Чат‑бот

Реализован отдельный контроллер `ChatBotController`, который принимает вопрос и возвращает ответ через сервис `GigaChatService`.

## 6) Структура репозитория (папки/файлы)

Ниже приведена структура проекта без служебных каталогов IDE (например `.vs`) и без временных артефактов. Названия приведены относительно корня репозитория.

### 6.1 Корень репозитория

- `PCConfigurator.api.sln` — solution файл.
- `docker-compose.yml` — запуск `postgres`, `api`, `client`.
- `PCConfigurator.api/` — серверная часть (ASP.NET Core).
- `pcconfigurator-client/` — клиентская часть (React).
- `dump.sql` — SQL‑дамп (если используется для переноса/резерва).
- `structure.txt`, `text.txt` — вспомогательные файлы проекта (если используются в учебных целях).

### 6.2 Backend: `PCConfigurator.api/`

- `Program.cs` — конфигурация DI, CORS, JWT, Swagger; запуск приложения и сидинг.
- `appsettings.json`, `appsettings.Development.json` — конфиги приложения.
- `Dockerfile` — сборка/публикация API контейнера.

#### 6.2.1 `Controllers/`

- `AuthController.cs` — регистрация/логин, выдача JWT.
- `CPUController.cs`, `GPUController.cs`, `RAMController.cs`, `SSDController.cs`, `HDDController.cs`, `PSUController.cs`, `CaseController.cs`, `ThermalPasteController.cs`, `MOTHERBOARDController.cs` — CRUD‑эндпоинты для каталогов комплектующих.
- `PCBuildController.cs` — получение и сохранение сборок (защищено `[Authorize]`).
- `PCBuildLikeController.cs` — лайки сборок.
- `ChatBotController.cs` — эндпоинт чат‑бота.

#### 6.2.2 `Models/`

- `ApplicationUser.cs` — расширение пользователя Identity (добавлены `FullName`, `CreatedAt`).
- `CPU.cs`, `GPU.cs`, `RAM.cs`, `SSD.cs`, `HDD.cs`, `PSU.cs`, `Case.cs`, `Motherboard.cs`, `ThermalPaste.cs` — доменные модели комплектующих.
- `PCBuild.cs` — модель сборки.
- `PCBuildLike.cs` — модель лайка.
- `PCBuildCreateDto.cs` — DTO для сохранения сборки.
- `DTO/PCBuildDto.cs` — DTO для отдачи сборок клиенту (включает компоненты и количество лайков).

#### 6.2.3 `Data/`

- `ApplicationDbContext.cs` — контекст EF Core + DbSet.
- `DatabaseSeeder.cs` — загрузка сид‑данных из JSON в таблицы комплектующих.
- `DbInitializer.cs` — создание ролей и администратора.

#### 6.2.4 `Migrations/`

- миграции EF Core + `ApplicationDbContextModelSnapshot.cs` (актуальная схема).

#### 6.2.5 `SeedData/`

JSON‑файлы начального наполнения каталога:

- `cpu.json`, `gpu.json`, `ram.json`, `motherboard.json`, `ssd.json`, `hdd.json`, `psu.json`, `case.json`, `thermalpaste.json`.

### 6.3 Frontend: `pcconfigurator-client/`

- `package.json` — зависимости и скрипты CRA.
- `Dockerfile` — multi‑stage сборка: `node` → `nginx`.
- `nginx/default.conf` — конфиг nginx для SPA fallback.

#### 6.3.1 `src/`

- `App.js` — маршрутизация приложения (React Router), подключение `/admin`.
- `index.js` — точка входа React.
- `components/` — основные компоненты UI:

  - `MainPage.js` — основной экран конфигуратора (категории, карточки, движок совместимости, сохранение сборки).
  - `ProtectedRoute.js` — защита маршрутов по токену и роли.
  - `AdminPanel.js` + формы `CPUForm.js`, `GPUForm.js`, `RAMForm.js`, `MBForm.js`, `SSDForm.js`, `HDDForm.js`, `PSUForm.js`, `CaseForm.js`, `ThermalPasteForm.js` — добавление комплектующих.
  - `Login.js`, `Register.js` — авторизация.
  - `ConfigurationsPage.js` — просмотр сохранённых конфигураций.
  - `GuidesPage.js`, `guidesData.js` — раздел с гайдами.
  - `ChatBot.js` — интерфейс чат‑бота.
  - `Header.js` — шапка/навигация.

## 7) Схема базы данных (PostgreSQL, реальная схема из EF Core)

Ниже перечислены таблицы и реальные поля по текущей схеме (снято из `Migrations/ApplicationDbContextModelSnapshot.cs` и моделей). Типы указаны как типы PostgreSQL (`HasColumnType(...)`).

### 7.1 Таблицы ASP.NET Identity

Эти таблицы создаются ASP.NET Core Identity и используются для хранения пользователей, ролей и связей.

#### 7.1.1 `AspNetUsers`

- **Id**: `text` — PK. Идентификатор пользователя (строка).
- **UserName**: `character varying(256)` — логин.
- **NormalizedUserName**: `character varying(256)` — нормализованный логин (для поиска).
- **Email**: `character varying(256)` — email.
- **NormalizedEmail**: `character varying(256)`.
- **EmailConfirmed**: `boolean`.
- **PasswordHash**: `text`.
- **SecurityStamp**: `text`.
- **ConcurrencyStamp**: `text`.
- **PhoneNumber**: `text`.
- **PhoneNumberConfirmed**: `boolean`.
- **TwoFactorEnabled**: `boolean`.
- **LockoutEnd**: `timestamp with time zone` (nullable).
- **LockoutEnabled**: `boolean`.
- **AccessFailedCount**: `integer`.
- **FullName**: `text` — доп. поле проекта.
- **CreatedAt**: `timestamp with time zone` — доп. поле проекта.

#### 7.1.2 `AspNetRoles`

- **Id**: `text` — PK.
- **Name**: `character varying(256)`.
- **NormalizedName**: `character varying(256)` — уникальный индекс.
- **ConcurrencyStamp**: `text`.

#### 7.1.3 `AspNetUserRoles`

Таблица связи пользователей и ролей.

- **UserId**: `text` — PK (часть составного ключа), FK → `AspNetUsers(Id)`.
- **RoleId**: `text` — PK (часть составного ключа), FK → `AspNetRoles(Id)`.

#### 7.1.4 `AspNetUserClaims`

- **Id**: `integer` — PK.
- **UserId**: `text` — FK → `AspNetUsers(Id)`.
- **ClaimType**: `text`.
- **ClaimValue**: `text`.

#### 7.1.5 `AspNetRoleClaims`

- **Id**: `integer` — PK.
- **RoleId**: `text` — FK → `AspNetRoles(Id)`.
- **ClaimType**: `text`.
- **ClaimValue**: `text`.

#### 7.1.6 `AspNetUserLogins`

- **LoginProvider**: `text` — PK (часть составного ключа).
- **ProviderKey**: `text` — PK (часть составного ключа).
- **ProviderDisplayName**: `text`.
- **UserId**: `text` — FK → `AspNetUsers(Id)`.

#### 7.1.7 `AspNetUserTokens`

- **UserId**: `text` — PK (часть составного ключа), FK → `AspNetUsers(Id)`.
- **LoginProvider**: `text` — PK (часть составного ключа).
- **Name**: `text` — PK (часть составного ключа).
- **Value**: `text`.

### 7.2 Таблицы каталога комплектующих

Все таблицы комплектующих имеют одинаковую базовую идею: строковые поля описания, цену и ссылку на изображение.

#### 7.2.1 `CPUs`

- **Id**: `integer` — PK.
- **Name**: `text` — название процессора.
- **Manufacturer**: `text` — производитель (например Intel/AMD).
- **Socket**: `text` — сокет.
- **Cores**: `integer` — количество ядер.
- **Threads**: `integer` — количество потоков.
- **BaseFrequency**: `real` — базовая частота.
- **BoostFrequency**: `real` — boost частота.
- **TDP**: `integer` — энергопотребление (Вт), используется в оценке БП.
- **IntegratedGraphics**: `text` — наличие/описание iGPU.
- **Price**: `numeric` — цена.
- **ImageUrl**: `text` — путь/URL к изображению.
- **ShortDescription**: `text` — краткое описание.

#### 7.2.2 `GPUs`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **MemorySize**: `integer` — объём памяти (ГБ).
- **MemoryType**: `text`.
- **PowerConsumption**: `integer` — потребление (Вт), используется в оценке БП.
- **PowerConnector**: `text` — тип/разъём питания.
- **Length**: `real` — длина (мм).
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

#### 7.2.3 `RAMs`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **Type**: `text` — тип памяти (DDR4/DDR5).
- **Capacity**: `integer` — объём (на модуль).
- **Modules**: `integer` — количество модулей.
- **Frequency**: `integer` — частота (МГц).
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

#### 7.2.4 `Motherboards`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **Socket**: `text` — сокет процессора.
- **Chipset**: `text` — чипсет.
- **FormFactor**: `text` — форм‑фактор.
- **RAMSlots**: `integer` — количество слотов RAM.
- **RAMType**: `text` — поддерживаемый тип RAM.
- **MaxRAM**: `integer` — в текущих данных используется как максимальная частота RAM.
- **HasM2**: `boolean` — наличие поддержки M.2.
- **HasPCIe5**: `boolean` — наличие PCIe 5.0.
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

#### 7.2.5 `SSDs`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **Capacity**: `integer` — объём (ГБ).
- **Interface**: `text` — интерфейс (SATA/NVMe).
- **FormFactor**: `text` — форм‑фактор (2.5", M.2).
- **ReadSpeed**: `integer` — чтение.
- **WriteSpeed**: `integer` — запись.
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

#### 7.2.6 `HDDs`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **Capacity**: `integer`.
- **RPM**: `integer`.
- **Cache**: `integer`.
- **Interface**: `text`.
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

#### 7.2.7 `PSUs`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **Power**: `integer` — мощность (Вт), используется в проверке совместимости.
- **FormFactor**: `text`.
- **Efficiency**: `text`.
- **Modular**: `boolean`.
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

#### 7.2.8 `Cases`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **FormFactor**: `text`.
- **MaxGPULength**: `real`.
- **MaxCPUCoolerHeight**: `real`.
- **FanSupport**: `integer`.
- **HasRGB**: `boolean`.
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

#### 7.2.9 `ThermalPastes`

- **Id**: `integer` — PK.
- **Name**: `text`.
- **Manufacturer**: `text`.
- **ThermalConductivity**: `real`.
- **Volume**: `real`.
- **Price**: `numeric`.
- **ImageUrl**: `text`.
- **ShortDescription**: `text`.

### 7.3 Таблицы пользовательских данных проекта

#### 7.3.1 `PCBuilds`

Таблица сохранённых сборок.

- **Id**: `integer` — PK.
- **Name**: `text` — название сборки.
- **CPUId**: `integer` nullable — FK → `CPUs(Id)`.
- **GPUId**: `integer` nullable — FK → `GPUs(Id)`.
- **RAMId**: `integer` nullable — FK → `RAMs(Id)`.
- **MotherboardId**: `integer` nullable — FK → `Motherboards(Id)`.
- **SSDId**: `integer` nullable — FK → `SSDs(Id)`.
- **HDDId**: `integer` nullable — FK → `HDDs(Id)`.
- **PSUId**: `integer` nullable — FK → `PSUs(Id)`.
- **CaseId**: `integer` nullable — FK → `Cases(Id)`.
- **ThermalPasteId**: `integer` nullable — FK → `ThermalPastes(Id)`.
- **TotalPrice**: `numeric` — итоговая стоимость, рассчитывается при сохранении.
- **UserId**: `text` — FK → `AspNetUsers(Id)` (владелец сборки).
- **CreatedAt**: `timestamp with time zone` — дата создания.

#### 7.3.2 `PCBuildLikes`

Таблица лайков сборок.

- **Id**: `integer` — PK.
- **BuildId**: `integer` — FK → `PCBuilds(Id)`.
- **UserId**: `text` — FK → `AspNetUsers(Id)`.
- **CreatedAt**: `timestamp with time zone`.

## 8) Как запустить (кратко)

Типовой запуск через Docker Compose:

- PostgreSQL поднимается на `localhost:5432`.
- API доступно на `http://localhost:8080`.
- Клиент (nginx) доступен на `http://localhost:3000`.

Маршруты:

- Главная (конфигуратор): `/`
- Админка: `/admin` (требуется роль Admin)

---

Этот файл можно использовать как «паспорт проекта» для курсовой работы: он описывает назначение, технологии, устройство репозитория и реальную схему БД с PK/FK.
