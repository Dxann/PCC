import React, { useState } from "react";
import { guides } from "./guidesData";
import "./GuidesPage.css";

export default function GuidesPage() {
  const [selectedGuide, setSelectedGuide] = useState(null);

  return (
    <div className="guides-page">
      {/* Показываем заголовок только когда НЕ выбран гайд */}
      {!selectedGuide && (
        <div className="guides-header">
          <h1>Гайды по сборке ПК</h1>
          <p>Подробные инструкции для разных целей и бюджетов</p>
        </div>
      )}

      {selectedGuide ? (
        <div className="guide-detail">
          <button 
            className="back-btn"
            onClick={() => setSelectedGuide(null)}
          >
            ← Назад к гайдам
          </button>
          
          <div className="guide-content">
            <h2>{selectedGuide.title}</h2>
            <div className="guide-meta">
              <span>Сложность: {selectedGuide.difficulty}</span>
              <span>Время: {selectedGuide.time}</span>
            </div>
            <div 
              className="guide-text"
              dangerouslySetInnerHTML={{ __html: selectedGuide.content }}
            />
          </div>
        </div>
      ) : (
        <div className="guides-grid">
          {guides.map(guide => (
            <div 
              key={guide.id} 
              className="guide-card"
              onClick={() => setSelectedGuide(guide)}
            >
              <div className="guide-image">
                <img src={guide.image} alt={guide.title} />
              </div>
              <div className="guide-info">
                <h3>{guide.title}</h3>
                <p>{guide.description}</p>
                <div className="guide-meta">
                  <span className="difficulty">{guide.difficulty}</span>
                  <span className="time">⏱ {guide.time}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}