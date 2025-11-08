import React, { useState, useEffect } from "react";
import axios from "axios";
import "./MainPage.css";

const categories = [
  { id: "CPU", name: "Процессоры", icon: "/images/cpu.png" },
  { id: "GPU", name: "Видеокарты", icon: "/images/gpu.png" },
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

  useEffect(() => {
    if (!selectedCategory) return;
    axios
      .get(`https://localhost:7200/api/${selectedCategory}`)
      .then(res => setItems(res.data))
      .catch(err => console.error("Ошибка загрузки:", err));
  }, [selectedCategory]);

  return (
    <div className="main-page">
      {/* Левая колонка */}
      <div className="sidebar">
        <h2>Категории</h2>
        <ul>
          {categories.map(cat => (
            <li key={cat.id}>
              <a onClick={() => setSelectedCategory(cat.id)}>
                <img src={cat.icon} alt={cat.name} />
                {cat.name}
              </a>
            </li>
          ))}
        </ul>
      </div>

      {/* Центральная часть */}
      <div className="items">
        <h2>{selectedCategory ? selectedCategory : "Выберите категорию"}</h2>
        {items.length === 0 ? (
          selectedCategory && <p>Нет данных</p>
        ) : (
          <ul>
            {items.map(item => (
              <li key={item.id}>
                {item.name} — {item.price}₽
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}
