import React, { useState } from "react";
import axios from "axios";
import "./Auth.css";

export default function Register() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState(""); 
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [message, setMessage] = useState("");

  const handleRegister = async (e) => {
    e.preventDefault();
    setMessage("");

    if (password !== confirmPassword) {
      setMessage("⚠ Пароли не совпадают.");
      return;
    }

    try {
      const res = await axios.post("https://localhost:7200/api/Auth/register", {
        username,
        email,
        password,
      });

      if (res.status === 200) {
        setMessage("✅ Регистрация успешна! Теперь войдите в аккаунт.");
        setUsername("");
        setEmail("");
        setPassword("");
        setConfirmPassword("");
      }
    } catch (err) {
        console.error(err);

        if (err.response?.status === 400) {
            const data = err.response.data;

            let errorsArray = [];

            if (Array.isArray(data)) errorsArray = data.map(e => e.description);
            else if (data?.errors && Array.isArray(data.errors))
            errorsArray = data.errors.map(e => e.description);
            else if (typeof data === "string") errorsArray = [data];

            const combined = errorsArray.join(" ");

            if (
            combined.includes("Passwords must be at least 6 characters") &&
            combined.includes("one non alphanumeric character") &&
            combined.includes("one lowercase") &&
            combined.includes("one uppercase")
            ) {
            setMessage(
                "⚠ Пароль должен быть не менее 6 символов, содержать хотя бы одну заглавную букву, одну строчную, цифру и спецсимвол."
            );
            } 
            
            else if (combined.includes("User already exists")) {
            setMessage("⚠ Пользователь уже существует.");
            } 
            else {
            setMessage(`⚠ Ошибки: ${combined}`);
            }
        } else {
            setMessage("❌ Ошибка при регистрации.");
        }
        }

  };

  return (
    <div className="auth-container">
      <h2>Регистрация</h2>
      <form onSubmit={handleRegister}>
        <input
          type="text"
          placeholder="Имя пользователя"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Подтверждение пароля"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
        />
        <button type="submit">Зарегистрироваться</button>
      </form>
      {message && <p className="auth-message">{message}</p>}
    </div>
  );
}
