import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Header from "./components/Header";
import PCBuilder from "./components/PCBuilder";
import MainPage from "./components/MainPage";

function Home() {
  return <MainPage />;
}

function Configurations() {
  return <h2>Здесь будут сборки</h2>;
}

function Guides() {
  return <h2>Здесь будут гайды</h2>;
}

function ChatBot() {
  return <h2>Здесь будет GIGACHAT</h2>;
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
        </Routes>
      </div>
    </Router>
  );
}

export default App;
