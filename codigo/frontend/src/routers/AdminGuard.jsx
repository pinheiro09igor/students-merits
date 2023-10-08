import React from "react";
import { Navigate } from "react-router-dom";

const AdminGuard = ({ children, navigate }) => {
  const isAuthenticated = true; /* Verifique se o usuário está autenticado e é um admin */

  if (!isAuthenticated) {
    navigate("/login"); // Redireciona o usuário não autenticado para a página de login
    return null;
  }

  // Se o usuário estiver autenticado como admin, renderiza as rotas protegidas
  return children;
};

export default AdminGuard;
