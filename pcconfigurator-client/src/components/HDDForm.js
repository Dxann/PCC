import React, { useState } from "react";
import axios from "axios";

export default function HDDForm({ onAdded }) {
  const [name, setName] = useState("");
  const [capacitygb, setCapacityGB] = useState("");
  const [price, setPrice] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post("https://localhost:7200/api/HDD", {
        name,
        capacitygb: parseInt(capacitygb),
        price: parseFloat(price),
      });
      onAdded(res.data);
      setName(""); setCapacityGB(""); setPrice("");
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>
      <h4>HDD накопитель</h4>
      <input placeholder="Название" value={name} onChange={e => setName(e.target.value)} required />
      <input placeholder="Память" type="number" value={capacitygb} onChange={e => setCapacityGB(e.target.value)} required />
      <input placeholder="Цена" type="number" value={price} onChange={e => setPrice(e.target.value)} required />
      <button type="submit">Добавить</button>
    </form>
  );
}
