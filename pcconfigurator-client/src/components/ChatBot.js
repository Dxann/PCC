import { useState } from "react";
import "./ChatBot.css";

function ChatBot() {
  const [question, setQuestion] = useState("");
  const [messages, setMessages] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  // Функция для преобразования текста в HTML с разметкой
  const formatMessage = (text) => {
    if (!text) return "";

    return text
      // Убираем ###
      .replace(/### /g, '')
      // Заголовки (текст перед списком) -> h4
      .replace(/([А-Я][А-Яа-я\s\-]+:)(\s•)/g, '<h4>$1</h4>$2')
      // Добавляем перенос перед • которые идут сразу после текста
      .replace(/([а-яё])([\s\u00A0]*•[\s\u00A0]*)([А-Я])/gi, '$1</p><p>• $3')
      // Убираем нумерованные списки (1. 2. 3.) - преобразуем в обычные пункты
      .replace(/(\d+)\.\s+/g, '• ')
      // Заголовки (текст на отдельной строке) -> h4
      .replace(/(\n|^)([А-Я][А-Яа-я\s\-]+)(\n|$)/g, '$1<h4>$2</h4>$3')
      // Маркированные списки с дефисами -> li
      .replace(/^-\s+(.+)$/gm, '<li>$1</li>')
      // Группируем li в ul
      .replace(/(<li>.*<\/li>)+/g, (match) => `<ul>${match}</ul>`)
      // Жирный текст **текст** -> <strong>
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      // Убираем лишние переносы и пробелы
      .replace(/\n\s*\n/g, '</p><p>')
      .replace(/\n/g, ' ')
      .replace(/\s+/g, ' ')
      // Оборачиваем в параграф
      .replace(/^(.*)$/, '<p>$1</p>')
      // Чистим лишние теги
      .replace(/<p><\/p>/g, '')
      .replace(/<p><h4/g, '<h4')
      .replace(/<\/h4><\/p>/g, '</h4>')
      .replace(/<p><ul/g, '<ul')
      .replace(/<\/ul><\/p>/g, '</ul>')
      .replace(/<p>\s*<li/g, '<ul><li')
      .replace(/<\/li>\s*<\/p>/g, '</li></ul>')

      .trim();
  };

  const send = async () => {
    if (!question.trim() || isLoading) return;

    const myMsg = { from: "user", text: question };
    setMessages(prev => [...prev, myMsg]);
    setIsLoading(true);
    setQuestion("");

    try {
      const res = await fetch("http://localhost:8080/api/ChatBot", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Accept": "*/*"
        },
        body: JSON.stringify({ question: question })
      });

      if (!res.ok) {
        throw new Error(`HTTP error! status: ${res.status}`);
      }

      const data = await res.json();
      const botMsg = { 
        from: "bot", 
        text: data.answer,
        html: formatMessage(data.answer)
      };
      setMessages(prev => [...prev, botMsg]);
      
    } catch (error) {
      console.error("Fetch error:", error);
      const errorMsg = { 
        from: "bot", 
        text: "Извините, произошла ошибка. Попробуйте позже." 
      };
      setMessages(prev => [...prev, errorMsg]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === 'Enter' && !isLoading) {
      send();
    }
  };

  return (
    <div className="chatbot-container">
      <div className="chatbot-header">
        <div className="header-content">
          <img src="/images/bot-avatar.png" alt="Bot" className="header-avatar" />
          <div className="header-info">
            <h3>GIGACHAT помощник</h3>
            <span className="status">Online</span>
          </div>
        </div>
      </div>

      <div className="chatbot-messages">
        {messages.length === 0 && (
          <div className="welcome-message">
            <img src="/images/bot-avatar.png" alt="Bot" className="welcome-avatar" />
            <div className="welcome-text">
              <h3>Привет! Я GIGACHAT помощник</h3>
              <p>Задавайте вопросы о сборке компьютеров, компонентах и совместимости</p>
            </div>
          </div>
        )}
        
        {messages.map((m, i) => (
          <div key={i} className={`message ${m.from === "user" ? "user-message" : "bot-message"}`}>
            <img 
              src={m.from === "user" ? "/images/user-avatar.png" : "/images/bot-avatar.png"} 
              alt="Avatar" 
              className="message-avatar" 
            />
            <div className="message-content">
              <div className="message-bubble">
                {m.from === "user" ? (
                  m.text
                ) : (
                  <div 
                    className="bot-message-content"
                    dangerouslySetInnerHTML={{ __html: m.html || m.text }}
                  />
                )}
              </div>
            </div>
          </div>
        ))}
        
        {isLoading && (
          <div className="message bot-message">
            <img src="/images/bot-avatar.png" alt="Bot" className="message-avatar" />
            <div className="message-content">
              <div className="message-bubble typing">
                <div className="typing-indicator">
                  <span></span>
                  <span></span>
                  <span></span>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>

      <div className="chatbot-input">
        <div className="input-container">
          <input
            value={question}
            onChange={(e) => setQuestion(e.target.value)}
            onKeyPress={handleKeyPress}
            placeholder="Введите ваш вопрос о сборке ПК..."
            disabled={isLoading}
          />
          <button 
            onClick={send} 
            disabled={isLoading || !question.trim()}
            className="send-button"
          >
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
              <path d="M2 21L23 12L2 3V10L17 12L2 14V21Z" fill="currentColor"/>
            </svg>
          </button>
        </div>
      </div>
    </div>
  );
}

export default ChatBot;