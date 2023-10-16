import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import { RequiredAuth } from "./context/auth/RequiredAuth";
import Login from "./pages/Login";
import RegistrarEmpresa from "./pages/RegisterEmpresa";
import RegistrarAluno from "./pages/RegisterAluno";

export function App() {
  // ----------------------- Return da função App -----------------------

  return (
    <>
      <Routes>
        <Route path="/aluno/registrar" element={<RegistrarAluno />} />
        <Route path="/empresa/registrar" element={<RegistrarEmpresa />} />
        <Route path="/logar" element={<Login />} />
        <Route index element={<Home />} />
        <Route
          path="dashboard"
          element={
            <RequiredAuth>
              <Dashboard />
            </RequiredAuth>
          }
        />
      </Routes>
    </>
  );
}

export default App;
