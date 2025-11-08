import React, { useState } from "react";
import axios from "axios";

export default function PSUForm({ onAdded }) {
  const [name, setName] = useState("");
  const [wattage, setWattage] = useState("");
  const [price, setPrice] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post("https://localhost:7200/api/PSU", {
        name,
        cores: parseInt(wattage),
        price: parseFloat(price),
      });
      onAdded(res.data);
      setName(""); setWattage(""); setPrice("");
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>
      <h4>Блок питания</h4>
      <input placeholder="Название" value={name} onChange={e => setName(e.target.value)} required />
      <input placeholder="Вольтаж" type="number" value={wattage} onChange={e => setWattage(e.target.value)} required />
      <input placeholder="Цена" type="number" value={price} onChange={e => setPrice(e.target.value)} required />
      <button type="submit">Добавить</button>
    </form>
  );
}
