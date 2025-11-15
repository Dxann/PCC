import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import PCBuilder from "./components/PCBuilder";
import MainPage from "./components/MainPage";
import AdminPanel from "./components/AdminPanel";
import ProtectedRoute from "./components/ProtectedRoute";
import Login from "./components/Login";
import Register from "./components/Register";
import ChatBot from "./components/ChatBot";

function Home() {
  return <MainPage />;
}

function Configurations() {
  return <h2>Здесь будут сборки</h2>;
}

function Guides() {
  return <h2>Здесь будут гайды</h2>;
}


function App() {
  return (
    <Router>
      <Header />
      <div style={{ padding: "30px", paddingTop: "10px" }}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/configurations" element={<Configurations />} />
          <Route path="/guides" element={<Guides />} />
          <Route path="/chatbot" element={<ChatBot />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          {/* Защищённый маршрут для админки с проверкой роли */}
          <Route
            path="/admin"
            element={
              <ProtectedRoute role="Admin">
                <AdminPanel />
              </ProtectedRoute>
            }
          />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
