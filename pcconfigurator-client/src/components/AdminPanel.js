import React, { useEffect, useState } from "react";
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
import ItemList from "./ItemList";

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

  const fetchAll = async () => {
    try {
      const [cpuRes, gpuRes, ramRes, ssdRes, hddRes, psuRes, mbRes, caseRes, thermalRes] = await Promise.all([
        axios.get("https://localhost:7200/api/CPU"),
        axios.get("https://localhost:7200/api/GPU"),
        axios.get("https://localhost:7200/api/RAM"),
        axios.get("https://localhost:7200/api/SSD"),
        axios.get("https://localhost:7200/api/HDD"),
        axios.get("https://localhost:7200/api/PSU"),
        axios.get("https://localhost:7200/api/MB"),
        axios.get("https://localhost:7200/api/Case"),
        axios.get("https://localhost:7200/api/ThermalPaste"),
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
      console.error(err);
    }
  };

  useEffect(() => { fetchAll(); }, []);

  return (
    <div style={{ padding: "20px" }}>
      <h2>Admin Panel</h2>

      <CPUForm onAdded={(item) => { setCpus([...cpus, item]); }} />
      <GPUForm onAdded={(item) => { setGpus([...gpus, item]); }} />
      <RAMForm onAdded={(item) => { setRams([...rams, item]); }} />
      <SSDForm onAdded={(item) => { setSsds([...ssds, item]); }} />
      <HDDForm onAdded={(item) => { setHdds([...hdds, item]); }} />
      <PSUForm onAdded={(item) => { setPsus([...psus, item]); }} />
      <MBForm onAdded={(item) => { setMbs([...mbs, item]); }} />
      <CaseForm onAdded={(item) => { setCases([...cases, item]); }} />
      <ThermalPasteForm onAdded={(item) => { setThermal([...thermal, item]); }} />

      {/* <ItemList items={cpus} title="CPUs" />
      <ItemList items={gpus} title="GPUs" /> */}
      {/* <ItemList items={rams} title="RAMs" />
      <ItemList items={ssds} title="SSDs" />
      <ItemList items={hdds} title="HDDs" />
      <ItemList items={psus} title="PSUs" />
      <ItemList items={mbs} title="Motherboards" />
      <ItemList items={cases} title="Cases" />
      <ItemList items={thermal} title="Thermal Pastes" /> */}
    </div>
  );
}
