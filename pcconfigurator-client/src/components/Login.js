import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./Auth.css";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setMessage("");

    try {
      const res = await axios.post("https://localhost:7200/api/Auth/login", {
        username,
        password,
      });

      // достаём роль (учитываем оба варианта — roles[] или role)
      const role =
        res.data.roles && Array.isArray(res.data.roles)
          ? res.data.roles[0]
          : res.data.role || "";

      // сохраняем токен и данные
      localStorage.setItem("token", res.data.token);
      localStorage.setItem("username", res.data.username);
      localStorage.setItem("role", role);

      setMessage("✅ Вход выполнен успешно!");
      setUsername("");
      setPassword("");

      // через секунду после успешного входа — редирект
      setTimeout(() => navigate("/"), 1000);
    } catch (err) {
      console.error(err);

      if (err.response?.status === 401) {
        setMessage("⚠ Неверный логин или пароль.");
      } else if (err.response?.status === 403) {
        setMessage("⚠ Доступ запрещён. Недостаточно прав.");
      } else if (err.response?.status === 500) {
        setMessage("❌ Ошибка сервера при входе.");
      } else {
        setMessage("❌ Не удалось войти. Проверьте соединение с сервером.");
      }
    }
  };

  return (
    <div className="auth-container">
      <h2>Вход</h2>
      <form onSubmit={handleLogin}>
        <input
          type="text"
          placeholder="Имя пользователя"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <button type="submit">Войти</button>
      </form>
      {message && <p className="auth-message">{message}</p>}
    </div>
  );
}
