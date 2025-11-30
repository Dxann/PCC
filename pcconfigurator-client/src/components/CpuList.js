import React, { useEffect, useState } from 'react';
import axios from 'axios';

function CpuList() {
  const [cpus, setCpus] = useState([]);

  useEffect(() => {
    axios.get('http://localhost:5001/api/cpus')
      .then(response => setCpus(response.data))
      .catch(error => console.error('Ошибка при получении данных:', error));
  }, []);

  return (
    <div style={{ padding: 20 }}>
      <h2>Список процессоров</h2>
      {cpus.length === 0 ? (
        <p>Нет данных</p>
      ) : (
        <ul>
          {cpus.map(cpu => (
            <li key={cpu.id}>{cpu.name} — {cpu.price} ₽</li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default CpuList;
