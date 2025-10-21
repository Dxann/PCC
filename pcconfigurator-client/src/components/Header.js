import React from "react";
import { Link } from "react-router-dom";
import './Header.css';

function Header() {
  return (
    <div className="header">
      <div className="logo">PCConfigure</div>
      <div className="nav-links">
        <Link to="/">Главная</Link>
        <Link to="/configurations">Конфигурации</Link>
        <Link to="/guides">Гайды</Link>
        <Link to="/chatbot">Чат-бот</Link>
      </div>
    </div>
  );
}

export default Header;
