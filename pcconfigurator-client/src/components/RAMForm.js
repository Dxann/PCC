import React, { useState } from "react";
import axios from "axios";

export default function RAMForm({ onAdded }) {
  const [name, setName] = useState("");
  const [memorygb, setMemoryGB] = useState("");
  const [frequency, setFrequency] = useState("");
  const [price, setPrice] = useState("");
  const [typeddr, setTypeDDR] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post("https://localhost:7200/api/RAM", {
        name,
        memorygb: parseInt(memorygb),
        frequency: parseInt(frequency),
        price: parseFloat(price),
        typeddr: parseInt(typeddr),
      });
      onAdded(res.data);
      setName(""); setMemoryGB(""); setFrequency(""); setPrice(""); setTypeDDR("");
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>
      <h4>Оперативная память</h4>
      <input placeholder="Название" value={name} onChange={e => setName(e.target.value)} required />
      <input placeholder="Память" type="number" value={memorygb} onChange={e => setMemoryGB(e.target.value)} required />
      <input placeholder="Частота" type="number" value={frequency} onChange={e => setFrequency(e.target.value)} required />
      <input placeholder="Цена" type="number" value={price} onChange={e => setPrice(e.target.value)} required />
      <input placeholder="Тип ДДР" type="number" value={typeddr} onChange={e => setTypeDDR(e.target.value)} required />
      <button type="submit">Добавить</button>
    </form>
  );
}
