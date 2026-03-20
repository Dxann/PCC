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

  const normalizeKey = (value) => (value ?? "").toString().trim().toLowerCase().replace(/\s+/g, "");

  const pick = (obj, ...keys) => {
    for (const k of keys) {
      const v = obj?.[k];
      if (v !== undefined && v !== null) return v;
    }
    return undefined;
  };

  const getId = (x) => pick(x, "id", "Id");
  const getSocket = (x) => pick(x, "socket", "Socket");
  const getFormFactor = (x) => pick(x, "formFactor", "FormFactor");
  const getRamType = (x) => pick(x, "ramType", "RAMType");
  const getMaxRamFreq = (x) => pick(x, "maxRAM", "MaxRAM");
  const getHasM2 = (x) => pick(x, "hasM2", "HasM2");
  const getGpuLength = (x) => pick(x, "length", "Length");
  const getCaseMaxGpuLength = (x) => pick(x, "maxGPULength", "MaxGPULength");
  const getCpuTdp = (x) => pick(x, "tdp", "TDP");
  const getGpuPower = (x) => pick(x, "powerConsumption", "PowerConsumption");
  const getPsuPower = (x) => pick(x, "power", "Power");
  const getRamTypeFromRam = (x) => pick(x, "type", "Type");
  const getRamFrequency = (x) => pick(x, "frequency", "Frequency");
  const getSsdInterface = (x) => pick(x, "interface", "Interface");
  const getSsdFormFactor = (x) => pick(x, "formFactor", "FormFactor");

  const normalizeFormFactor = (value) => {
    const k = normalizeKey(value);
    if (!k) return "";
    if (k.includes("micro") || k.includes("matx") || k.includes("m-atx") || k.includes("m_atx") || k.includes("microatx")) return "matx";
    if (k.includes("mini") || k.includes("itx")) return "itx";
    if (k.includes("atx")) return "atx";
    return k;
  };

  const caseSupportsMotherboard = (pcCase, motherboard) => {
    const c = normalizeFormFactor(getFormFactor(pcCase));
    const m = normalizeFormFactor(getFormFactor(motherboard));
    if (!c || !m) return true;
    const rank = { itx: 1, matx: 2, atx: 3 };
    if (rank[c] == null || rank[m] == null) return c === m;
    return rank[c] >= rank[m];
  };

  const isNvmeOrM2 = (ssd) => {
    const iface = normalizeKey(getSsdInterface(ssd));
    const ff = normalizeKey(getSsdFormFactor(ssd));
    return iface.includes("nvme") || ff.includes("m.2") || ff.includes("m2");
  };

  const resultFromReasons = (reasons) => ({
    compatible: reasons.length === 0,
    reasons,
    reason: reasons.join("; ")
  });

  const getCompatibility = (category, item, currentBuild) => {
    const b = currentBuild;

    if (!item) return resultFromReasons([]);

    if (category === "CPU") {
      if (!b.Motherboard) return resultFromReasons([]);
      const reasons = [];
      const ok = normalizeKey(getSocket(item)) === normalizeKey(getSocket(b.Motherboard));
      if (!ok) reasons.push("Несовместимый сокет");
      return resultFromReasons(reasons);
    }

    if (category === "Motherboard") {
      const reasons = [];

      if (b.CPU) {
        const okSocket = normalizeKey(getSocket(item)) === normalizeKey(getSocket(b.CPU));
        if (!okSocket) reasons.push("Несовместимый сокет");
      }

      if (b.RAM) {
        const okType = normalizeKey(getRamType(item)) === normalizeKey(getRamTypeFromRam(b.RAM));
        if (!okType) reasons.push("Несовместимый тип памяти");
        const okFreq = Number(getMaxRamFreq(item)) >= Number(getRamFrequency(b.RAM));
        if (!okFreq) reasons.push("Слишком высокая частота памяти");
      }

      return resultFromReasons(reasons);
    }

    if (category === "RAM") {
      if (!b.Motherboard) return resultFromReasons([]);
      const reasons = [];
      const okType = normalizeKey(getRamTypeFromRam(item)) === normalizeKey(getRamType(b.Motherboard));
      if (!okType) reasons.push("Несовместимый тип памяти");
      const okFreq = Number(getRamFrequency(item)) <= Number(getMaxRamFreq(b.Motherboard));
      if (!okFreq) reasons.push("Слишком высокая частота памяти");
      return resultFromReasons(reasons);
    }

    if (category === "GPU") {
      return resultFromReasons([]);
    }

    if (category === "Case") {
      return resultFromReasons([]);
    }

    if (category === "PSU") {
      const cpuTdp = b.CPU ? Number(getCpuTdp(b.CPU)) : 0;
      const gpuPower = b.GPU ? Number(getGpuPower(b.GPU)) : 0;
      if (!cpuTdp && !gpuPower) return resultFromReasons([]);
      const required = cpuTdp + gpuPower + 150;
      const reasons = [];
      const ok = Number(getPsuPower(item)) >= required;
      if (!ok) reasons.push("Недостаточная мощность блока питания");
      return resultFromReasons(reasons);
    }

    if (category === "SSD") {
      if (!b.Motherboard) return resultFromReasons([]);
      if (!isNvmeOrM2(item)) return resultFromReasons([]);
      const reasons = [];
      const ok = Boolean(getHasM2(b.Motherboard));
      if (!ok) reasons.push("Нет поддержки M.2");
      return resultFromReasons(reasons);
    }

    if (category === "HDD") {
      return resultFromReasons([]);
    }

    if (category === "ThermalPaste") {
      return resultFromReasons([]);
    }

    return resultFromReasons([]);
  };

  const isItemCompatible = (category, item, currentBuild) => getCompatibility(category, item, currentBuild).compatible;

  const pruneIncompatible = (currentBuild) => {
    let b = { ...currentBuild };
    let changed = true;

    while (changed) {
      changed = false;

      const checks = [
        ["CPU", b.CPU],
        ["Motherboard", b.Motherboard],
        ["RAM", b.RAM],
        ["GPU", b.GPU],
        ["Case", b.Case],
        ["PSU", b.PSU],
        ["SSD", b.SSD],
        ["HDD", b.HDD],
        ["ThermalPaste", b.ThermalPaste]
      ];

      for (const [cat, val] of checks) {
        if (!val) continue;
        const ok = isItemCompatible(cat, val, b);
        if (!ok) {
          b = { ...b, [cat]: null };
          changed = true;
        }
      }
    }

    return b;
  };


  const saveBuild = async () => {
    if (!build.CPU || !build.GPU || !build.Motherboard) {
      setSaveMessage("Добавьте минимум CPU, GPU и Материнскую плату");
      return;
    }

    const payload = {
      Name: buildName || `Сборка ${new Date().toLocaleString()}`,
      CPUId: build.CPU?.id || null,
      GPUId: build.GPU?.id || null,
      RAMId: build.RAM?.id || null,
      MotherboardId: build.Motherboard?.id || null,
      SSDId: build.SSD?.id || null,
      HDDId: build.HDD?.id || null,
      PSUId: build.PSU?.id || null,
      CaseId: build.Case?.id || null,
      ThermalPasteId: build.ThermalPaste?.id || null,
    };

    try {
      const token = localStorage.getItem("token");

      await axios.post(
        "http://localhost:8080/api/PCBuild/save",
        payload,
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );

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
    setBuild(prev => {
      const newBuild = pruneIncompatible({ ...prev, [category]: item });
      return newBuild;
    });
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
      .get(`http://localhost:8080/api/${selectedCategory}`)
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
            {items.map(item => {
              const comp = getCompatibility(selectedCategory, item, build);
              return (
                <div
                  className={`card ${!comp.compatible ? "disabled" : ""}`}
                  key={item.id}
                  title={!comp.compatible ? comp.reason : ""}
                  onClick={() => {
                    if (!comp.compatible) return;
                    handleAddToBuild(selectedCategory, item);
                  }}
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
              );
            })}
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
