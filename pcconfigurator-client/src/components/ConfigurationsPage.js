import React, { useState, useEffect } from "react";
import axios from "axios";
import "./ConfigurationsPage.css";

export default function ConfigurationsPage() {
  const [activeTab, setActiveTab] = useState("all");
  const [builds, setBuilds] = useState([]);
  const [filteredBuilds, setFilteredBuilds] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [priceRange, setPriceRange] = useState({ min: 0, max: 1000000 });
  const [loading, setLoading] = useState(true);
  const [myLikes, setMyLikes] = useState(new Set());
  const [currentUsername, setCurrentUsername] = useState("");

  // Получаем имя текущего пользователя из localStorage
  useEffect(() => {
    const username = localStorage.getItem("username");
    if (username) {
      setCurrentUsername(username);
    }
  }, []);

  // Загрузка сборок
  useEffect(() => {
    loadBuilds();
    loadMyLikes();
  }, []);

  // Фильтрация и поиск
  useEffect(() => {
    let filtered = builds;

    // Фильтр по вкладкам
    if (activeTab === "my") {
      if (currentUsername) {
        const myBuilds = builds.filter(build => {
          const matches = build.userName === currentUsername;
          return matches;
        });
        filtered = myBuilds;
      } else {
        filtered = [];
      }
    } else if (activeTab === "liked") {
      filtered = filtered.filter(build => myLikes.has(build.id));
    }

    // Поиск по названию
    if (searchTerm) {
      filtered = filtered.filter(build =>
        build.name.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    // Фильтр по цене
    filtered = filtered.filter(build =>
      build.totalPrice >= priceRange.min && build.totalPrice <= priceRange.max
    );

    // Сортировка по лайкам (по умолчанию)
    filtered.sort((a, b) => b.likes - a.likes);

    setFilteredBuilds(filtered.slice(0, 30)); // Ограничение 30 карточек, другие искать по названию
  }, [builds, activeTab, searchTerm, priceRange, myLikes, currentUsername]);

  const loadBuilds = async () => {
    try {
      const token = localStorage.getItem("token");
      const response = await axios.get("https://localhost:7200/api/PCBuild", {
        headers: { Authorization: `Bearer ${token}` }
      });
      
      setBuilds(response.data);
      setLoading(false);
    } catch (error) {
      console.error("Ошибка загрузки сборок:", error);
      setLoading(false);
    }
  };

  const loadMyLikes = async () => {
    try {
      const token = localStorage.getItem("token");
      const response = await axios.get("https://localhost:7200/api/PCBuildLike/my", {
        headers: { Authorization: `Bearer ${token}` }
      });
      
      const likedBuildIds = response.data.map(like => like.buildId);
      setMyLikes(new Set(likedBuildIds));
    } catch (error) {
      console.error("Ошибка загрузки лайков:", error);
    }
  };

  const handleLike = async (buildId) => {
    try {
      const token = localStorage.getItem("token");
      const response = await axios.post(
        `https://localhost:7200/api/PCBuildLike/${buildId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );

      // Обновляем локальное состояние лайков
      const newLikes = new Set(myLikes);
      if (response.data.liked) {
        newLikes.add(buildId);
      } else {
        newLikes.delete(buildId);
      }
      setMyLikes(newLikes);

      // Обновляем счетчик лайков в сборках
      setBuilds(prevBuilds =>
        prevBuilds.map(build =>
          build.id === buildId
            ? {
                ...build,
                likes: response.data.liked ? build.likes + 1 : build.likes - 1
              }
            : build
        )
      );
    } catch (error) {
      console.error("Ошибка лайка:", error);
    }
  };

  const getComponentList = (build) => {
    const components = [];
    
    if (build.cpu && build.cpu.name) components.push(`CPU: ${build.cpu.name}`);
    if (build.gpu && build.gpu.name) components.push(`GPU: ${build.gpu.name}`);
    if (build.ram && build.ram.name) components.push(`RAM: ${build.ram.name}`);
    if (build.motherboard && build.motherboard.name) components.push(`MB: ${build.motherboard.name}`);
    if (build.ssd && build.ssd.name) components.push(`SSD: ${build.ssd.name}`);
    if (build.hdd && build.hdd.name) components.push(`HDD: ${build.hdd.name}`);
    if (build.psu && build.psu.name) components.push(`PSU: ${build.psu.name}`);
    if (build.case && build.case.name) components.push(`Case: ${build.case.name}`);
    if (build.thermalPaste && build.thermalPaste.name) components.push(`Paste: ${build.thermalPaste.name}`);
    
    return components;
  };

  if (loading) {
    return <div className="loading">Загрузка конфигураций...</div>;
  }

  return (
    <div className="configurations-page">
      {/* Заголовок и фильтры */}
      <div className="configurations-header">
        <h1>Конфигурации ПК</h1>
        
        {/* Переключение вкладок */}
        <div className="tabs">
          <button
            className={`tab ${activeTab === "all" ? "active" : ""}`}
            onClick={() => setActiveTab("all")}
          >
            Все конфигурации
          </button>
          <button
            className={`tab ${activeTab === "my" ? "active" : ""}`}
            onClick={() => {
              setActiveTab("my");
              console.log("Текущий username:", currentUsername);
            }}
          >
            Мои конфигурации
          </button>
          <button
            className={`tab ${activeTab === "liked" ? "active" : ""}`}
            onClick={() => setActiveTab("liked")}
          >
            Понравившиеся
          </button>
        </div>

        {/* Поиск и фильтры */}
        <div className="filters">
          <div className="search-box">
            <input
              type="text"
              placeholder="Поиск по названию сборки..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
            />
          </div>

          <div className="price-filter">
            <label>Цена: </label>
            <input
              type="text"
              placeholder="Мин"
              value={priceRange.min === 0 ? '' : priceRange.min}
              onChange={(e) => {
                const value = e.target.value.replace(/[^0-9]/g, ''); // Оставляем только цифры
                setPriceRange(prev => ({
                  ...prev,
                  min: value === '' ? 0 : parseInt(value) || 0
                }));
              }}
              className="price-input"
            />
            <span> - </span>
            <input
              type="text"
              placeholder="Макс"
              value={priceRange.max === 10000000 ? '' : priceRange.max}
              onChange={(e) => {
                const value = e.target.value.replace(/[^0-9]/g, ''); // Оставляем только цифры
                setPriceRange(prev => ({
                  ...prev,
                  max: value === '' ? 10000000 : parseInt(value) || 10000000
                }));
              }}
              className="price-input"
            />
          </div>
        </div>
      </div>

      {/* Сетка карточек */}
      <div className="builds-grid">
        {filteredBuilds.length === 0 ? (
          <div className="no-builds">
            {activeTab === "all" && "Нет сборок для отображения"}
            {activeTab === "my" && (currentUsername 
              ? "У вас пока нет сохраненных сборок" 
              : "Войдите в систему чтобы увидеть свои сборки")}
            {activeTab === "liked" && "Вы еще не лайкнули ни одной сборки"}
          </div>
        ) : (
          filteredBuilds.map(build => (
            <div key={build.id} className="build-card">
              {/* Заголовок и лайк */}
              <div className="build-card-header">
                <div className="build-title-wrapper">
                  <h3 className="build-name">
                    {/* Подсветка сборок пользователя */}
                    {build.userName === currentUsername && (
                      <span className="my-build-badge">⭐ </span>
                    )}
                    {build.name}
                  </h3>
                </div>
                <button
                  className={`like-btn ${myLikes.has(build.id) ? "liked" : ""}`}
                  onClick={(e) => {
                    e.stopPropagation();
                    handleLike(build.id);
                  }}
                >
                  <img
                    src={myLikes.has(build.id) ? "/images/liked.png" : "/images/unliked.png"}
                    alt="like"
                  />
                  <span>{build.likes}</span>
                </button>
              </div>

              {/* Цена */}
              <div className="build-price">{build.totalPrice.toLocaleString()} ₽</div>

              {/* Компоненты */}
              <div className="build-components">
                {getComponentList(build).map((component, index) => (
                  <div key={index} className="component-item">
                    {component}
                  </div>
                ))}
                {getComponentList(build).length === 0 && (
                  <div className="component-item">Компоненты не указаны</div>
                )}
              </div>

              {/* Информация о пользователе */}
              <div className="build-footer">
                <span className="build-user">{build.userName}</span>
                <span className="build-date">
                  {build.createdAt ? (
                    new Date(build.createdAt).toLocaleDateString('ru-RU', {
                      day: '2-digit',
                      month: '2-digit',
                      year: 'numeric'
                    })
                  ) : (
                    'Дата не указана'
                  )}
                </span>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
}