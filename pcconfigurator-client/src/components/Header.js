import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "./Header.css";

function Header() {
  const navigate = useNavigate();
  const token = localStorage.getItem("token");
  const username = localStorage.getItem("username");
  const role = localStorage.getItem("role");

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("username");
    localStorage.removeItem("role");
    navigate("/"); // перенаправление на главную после выхода
  };

  return (
    <div className="header">
      <div className="logo">PCConfigure</div>

      <div className="nav-links">
        <Link to="/">Главная</Link>
        <Link to="/configurations">Конфигурации</Link>
        <Link to="/guides">Гайды</Link>
        <Link to="/chatbot">Чат-бот</Link>

        <div className="auth-links">
          {token ? (
            <>
              <span className="user-info">
                {role === "Admin" ? (
                  <span>🛠 Администратор ({username})</span>
                ) : (
                  <span>👤 {username}</span>
                )}
              </span>
              <button className="logout-btn" onClick={handleLogout}>
                Выйти
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="login-btn">
                Войти
              </Link>
              <Link to="/register" className="register-btn">
                Регистрация
              </Link>
            </>
          )}
        </div>
      </div>
    </div>
  );
}

export default Header;
