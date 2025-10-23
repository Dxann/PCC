import React, { useState } from "react";
import axios from "axios";

export default function CaseForm({ onAdded }) {
  const [name, setName] = useState("");
  const [formfactor, setFormFactor] = useState("");
  const [price, setPrice] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post("https://localhost:7200/api/CPU", {
        name,
        formfactor: parseInt(formfactor),
        price: parseFloat(price),
      });
      onAdded(res.data);
      setName(""); setFormFactor(""); setPrice("");
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: "20px" }}>
      <h4>Корпус</h4>
      <input placeholder="Название" value={name} onChange={e => setName(e.target.value)} required />
      <input placeholder="Форма" type="number" value={formfactor} onChange={e => setFormFactor(e.target.value)} required />
      <input placeholder="Цена" type="number" value={price} onChange={e => setPrice(e.target.value)} required />
      <button type="submit">Добавить</button>
    </form>
  );
}
