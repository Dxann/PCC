import React, { useState, useEffect } from "react";
import axios from "axios";
import "./MainPage.css";

const categories = [
  { id: "CPU", name: "Процессор", icon: "/images/cpu.png" },
  { id: "GPU", name: "Видеокарта", icon: "/images/gpu.png" },
  { id: "RAM", name: "Оперативная память", icon: "/images/ram.png" },
  { id: "Motherboard", name: "Материнская плата", icon: "/images/mb.png" },
  { id: "SSD", name: "SSD", icon: "/images/ssd.png" },
  { id: "HDD", name: "Жёсткий диск", icon: "/images/hdd.png" },
  { id: "PSU", name: "Блок питания", icon: "/images/psu.png" },
  { id: "Case", name: "Корпус", icon: "/images/case.png" },
  { id: "ThermalPaste", name: "Термопаста", icon: "/images/thermal.png" }
];

export default function MainPage() {
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [items, setItems] = useState([]);
  const [nameError, setNameError] = useState("");
  const [saveMessage, setSaveMessage] = useState(null);

  const [buildName, setBuildName] = useState("");
  const [askName, setAskName] = useState(false);


  const saveBuild = async () => {
    if (!build.CPU || !build.GPU || !build.Motherboard) {
      setSaveMessage("Добавьте минимум CPU, GPU и Материнскую плату");
      return;
    }

    const payload = {
      name: buildName || `Сборка ${new Date().toLocaleString()}`,
      cpuId: build.CPU?.id || null,
      gpuId: build.GPU?.id || null,
      ramId: build.RAM?.id || null,
      motherboardId: build.Motherboard?.id || null,
      ssdId: build.SSD?.id || null,
      hddId: build.HDD?.id || null,
      psuId: build.PSU?.id || null,
      caseId: build.Case?.id || null,
      thermalPasteId: build.ThermalPaste?.id || null,
    };

    try {
      await axios.post("https://localhost:7200/api/PCBuild/save", payload);
      setSaveMessage("Сборка успешно сохранена!");
    } catch (err) {
      setSaveMessage("Ошибка при сохранении");
      console.error(err);
    }
  };

  const handleSaveClick = () => {

    const missing = Object.entries(build)
      .filter(([key, value]) => value === null)
      .map(([key]) => key);

    if (missing.length > 0) {
      setSaveMessage("Перед сохранением выберите все компоненты сборки!");
      return;
    }

    setSaveMessage(null);
    setAskName(true);
  };



  const [build, setBuild] = useState({
    CPU: null,
    GPU: null,
    RAM: null,
    Motherboard: null,
    SSD: null,
    HDD: null,
    PSU: null,
    Case: null,
    ThermalPaste: null
  });

  const handleAddToBuild = (category, item) => {
    setBuild(prev => ({ ...prev, [category]: item }));
  };

  const removeItem = (category) => {
    setBuild(prev => ({ ...prev, [category]: null }));
  };

  const clearBuild = () => {
    setBuild({
      CPU: null,
      GPU: null,
      RAM: null,
      Motherboard: null,
      SSD: null,
      HDD: null,
      PSU: null,
      Case: null,
      ThermalPaste: null
    });
  };

  const totalPrice = Object.values(build)
    .filter(x => x)
    .reduce((sum, item) => sum + item.price, 0);

  // главное изображение сборки
  const buildImage = "/images/pc.png";


  useEffect(() => {
    if (!selectedCategory) return;
    axios
      .get(`https://localhost:7200/api/${selectedCategory}`)
      .then(res => setItems(res.data))
      .catch(err => console.error("Ошибка:", err));
  }, [selectedCategory]);

  return (
    <div className="main-page">

      {saveMessage && (
        <div className="modal">
          <div className="modal-content">
            <p>{saveMessage}</p>
            <button onClick={() => setSaveMessage(null)}>Ок</button>
          </div>
        </div>
      )}

      {askName && (
        <div className="modal" onClick={(e) => e.stopPropagation()}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>

            <h3>Название сборки</h3>

            {nameError && ( 
              <p className="input-error">{nameError}</p>
            )}

            <input
              type="text"
              placeholder="Введите название"
              value={buildName}
              onChange={(e) => setBuildName(e.target.value)}
            />

            <div className="modal-buttons">
              <button
                className="build-btn"
                onClick={() => {
                  if (!buildName.trim()) {
                    setNameError("Введите название сборки!"); 
                    return;
                  }
                  setNameError("");
                  setAskName(false);
                  saveBuild();
                }}
              >
                Сохранить
              </button>

              <button 
              className="build-btn delete"
              onClick={() => {
                setAskName(false);
                setNameError("");
              }}>Отмена</button>
            </div>
          </div>
        </div>
      )}
      
      {/* Левое меню */}
      <div className="sidebar">
        <h2>Категории</h2>
        <ul>
          {categories.map(cat => (
            <li key={cat.id}>
              <a
                className={selectedCategory === cat.id && !askName && !saveMessage ? "active" : ""}
                onClick={() => setSelectedCategory(cat.id)}
              >
                <img src={cat.icon} alt={cat.name} />
                {cat.name}
              </a>
            </li>
          ))}
        </ul>
      </div>

      {/* Центральные карточки */}
      <div className="items">
        <h2>{selectedCategory || "Выберите категорию"}</h2>

        {items.length === 0 ? (
          selectedCategory && <p>Нет данных</p>
        ) : (
          <div className="grid">
            {items.map(item => (
              <div
                className="card"
                key={item.id}
                onClick={() => handleAddToBuild(selectedCategory, item)}
              >
                <img
                  className="card-image"
                  src={item.imageUrl || "/images/no-image.png"}
                  alt={item.name}
                />
                <h3 className="card-title">{item.name}</h3>
                <p className="card-desc">{item.shortDescription}</p>
                <div className="card-price">{item.price} ₽</div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Правая панель */}
      <div className="build-panel">

        <h2 className="build-title">Конфигурация</h2>

        <img className="build-main-image" src={buildImage} alt="config" />

        <div className="config-price">{totalPrice} ₽</div>

        <div className="build-buttons">
          
          <button
            className="build-btn"
            onClick={(e) => {
              e.stopPropagation();
              handleSaveClick();
            }}
          >
            <img src="/images/savebuild.png" alt="save" />
            Сохранить
          </button>

          <button className="build-btn delete" onClick={clearBuild}>
            <img src="/images/deletebuild.png" alt="delete" />
            Очистить
          </button>
        </div>

      <ul className="config-list">
        {categories.map(cat => (
          <li key={cat.id} className="config-row">

            <span className="config-name">{cat.name}:</span>

            {build[cat.id] ? (
              <div className="config-selected">
                <span className="config-selected-name">{build[cat.id].name}</span>
                <button className="remove-item" onClick={() => removeItem(cat.id)}>×</button>
              </div>
            ) : (
              <span className="config-empty">Не выбрано</span>
            )}

          </li>
        ))}
      </ul>


      </div>
    </div>
  );
}
