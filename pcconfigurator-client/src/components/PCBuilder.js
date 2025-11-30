import React, { useState, useEffect } from "react";
import axios from "axios";

export default function PCBuilder() {
  const [cpus, setCpus] = useState([]);
  const [gpus, setGpus] = useState([]);
  const [selectedCPU, setSelectedCPU] = useState(null);
  const [selectedGPU, setSelectedGPU] = useState(null);
  const [buildName, setBuildName] = useState("");

  useEffect(() => {
    axios.get("http://localhost:8080/api/cpu").then(res => setCpus(res.data));
    axios.get("http://localhost:8080/api/gpu").then(res => setGpus(res.data));
  }, []);

  const totalPrice = (selectedCPU?.price || 0) + (selectedGPU?.price || 0);

  const saveBuild = async () => {
    if (!selectedCPU || !selectedGPU || !buildName) return alert("Выберите всё и укажите имя сборки");

    await axios.post("http://localhost:8080/api/PCBuild", {
      CPUId: selectedCPU.id,
      GPUId: selectedGPU.id,
      Name: buildName,
      TotalPrice: totalPrice
    });

    alert("Сборка сохранена!");
  };

  return (
    <div>
      <h2>Сборка ПК</h2>
      <input
        type="text"
        placeholder="Имя сборки"
        value={buildName}
        onChange={(e) => setBuildName(e.target.value)}
      />
      <select onChange={(e) => setSelectedCPU(cpus.find(c => c.id === +e.target.value))}>
        <option value="">Выберите CPU</option>
        {cpus.map(cpu => <option key={cpu.id} value={cpu.id}>{cpu.name} - {cpu.price}₽</option>)}
      </select>
      <select onChange={(e) => setSelectedGPU(gpus.find(g => g.id === +e.target.value))}>
        <option value="">Выберите GPU</option>
        {gpus.map(gpu => <option key={gpu.id} value={gpu.id}>{gpu.name} - {gpu.price}₽</option>)}
      </select>
      <div>Итоговая цена: {totalPrice}₽</div>
      <button onClick={saveBuild}>Сохранить сборку</button>
    </div>
  );
}
