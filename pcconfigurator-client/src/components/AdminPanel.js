import React, { useState, useEffect } from "react";
import axios from "axios";
import CPUForm from "./CPUForm";
import GPUForm from "./GPUForm";
import RAMForm from "./RAMForm";
import SSDForm from "./SSDForm";
import HDDForm from "./HDDForm";
import PSUForm from "./PSUForm";
import MBForm from "./MBForm";
import CaseForm from "./CaseForm";
import ThermalPasteForm from "./ThermalPasteForm";
// import ItemList from "./ItemList";

export default function AdminPanel() {
  const [cpus, setCpus] = useState([]);
  const [gpus, setGpus] = useState([]);
  const [rams, setRams] = useState([]);
  const [ssds, setSsds] = useState([]);
  const [hdds, setHdds] = useState([]);
  const [psus, setPsus] = useState([]);
  const [mbs, setMbs] = useState([]);
  const [cases, setCases] = useState([]);
  const [thermal, setThermal] = useState([]);
  const [error, setError] = useState("");

  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");

  const fetchAll = async () => {
    if (!token) return;
    const headers = { Authorization: `Bearer ${token}` };

    try {
      // Загружаем все данные параллельно
      const [
        cpuRes,
        gpuRes,
        ramRes,
        ssdRes,
        hddRes,
        psuRes,
        mbRes,
        caseRes,
        thermalRes,
      ] = await Promise.all([
        axios.get("http://localhost:8080/api/CPU", { headers }),
        axios.get("http://localhost:8080/api/GPU", { headers }),
        axios.get("http://localhost:8080/api/RAM", { headers }),
        axios.get("http://localhost:8080/api/SSD", { headers }),
        axios.get("http://localhost:8080/api/HDD", { headers }),
        axios.get("http://localhost:8080/api/PSU", { headers }),
        axios.get("http://localhost:8080/api/Motherboard", { headers }),
        axios.get("http://localhost:8080/api/Case", { headers }),
        axios.get("http://localhost:8080/api/ThermalPaste", { headers }),
      ]);

      setCpus(cpuRes.data);
      setGpus(gpuRes.data);
      setRams(ramRes.data);
      setSsds(ssdRes.data);
      setHdds(hddRes.data);
      setPsus(psuRes.data);
      setMbs(mbRes.data);
      setCases(caseRes.data);
      setThermal(thermalRes.data);
    } catch (err) {
      console.error("Ошибка при загрузке данных:", err);
      setError("⚠ Ошибка при загрузке данных или нет доступа.");
    }
  };

  // Загружаем данные при монтировании и изменении токена
  useEffect(() => {
    if (token && role === "Admin") {
      fetchAll();
    }
  }, [token, role]);

  // Проверка токена и роли
  if (!token) {
    return <p>⚠ Нет доступа. Пожалуйста, войдите в систему.</p>;
  }

  if (role !== "Admin") {
    return <p>🚫 Доступ запрещён. Только для администраторов.</p>;
  }

  if (error) {
    return <p>{error}</p>;
  }

  return (
    <div style={{ padding: "20px" }}>
      <h2>Панель администратора</h2>

      <CPUForm onAdded={(item) => setCpus([...cpus, item])} />
      <GPUForm onAdded={(item) => setGpus([...gpus, item])} />
      <RAMForm onAdded={(item) => setRams([...rams, item])} />
      <SSDForm onAdded={(item) => setSsds([...ssds, item])} />
      <HDDForm onAdded={(item) => setHdds([...hdds, item])} />
      <PSUForm onAdded={(item) => setPsus([...psus, item])} />
      <MBForm onAdded={(item) => setMbs([...mbs, item])} />
      <CaseForm onAdded={(item) => setCases([...cases, item])} />
      <ThermalPasteForm onAdded={(item) => setThermal([...thermal, item])} />

      {/* При желании вернуть список компонентов */}
      {/* 
      <ItemList items={cpus} title="CPUs" />
      <ItemList items={gpus} title="GPUs" />
      <ItemList items={rams} title="RAMs" />
      ...
      */}
    </div>
  );
}
