import React, { useState } from "react";
import axios from "axios";

export default function MBForm({ onAdded }) {
  const [name, setName] = useState("");
  const [cores, setCores] = useState("");
  const [frequency, setFrequency] = useState("");
  const [price, setPrice] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post("https://localhost:7200/api/CPU", {
        name,
        cores: parseInt(cores),
        frequency: parseInt(frequency),
        price: parseFloat(price),
      });
      onAdded(res.data);
      setName(""); setCores(""); setFrequency(""); setPrice("");
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>
      <h4>Материнская плата</h4>
      <input placeholder="Название" value={name} onChange={e => setName(e.target.value)} required />
      <input placeholder="Ядра" type="number" value={cores} onChange={e => setCores(e.target.value)} required />
      <input placeholder="Частота" type="number" value={frequency} onChange={e => setFrequency(e.target.value)} required />
      <input placeholder="Цена" type="number" value={price} onChange={e => setPrice(e.target.value)} required />
      <button type="submit">Добавить</button>
    </form>
  );
}
