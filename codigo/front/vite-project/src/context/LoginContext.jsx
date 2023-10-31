import { useState, useEffect, createContext } from "react";
import { useNavigate } from "react-router-dom";
import {
  cadastrarUsuario,
  cadastrarEmpresa,
  obterUsuario,
  obterEmpresa,
  logarUsuario,
  logarEmpresa,
  deslogarUsuario,
  LoginService,
} from "../hooks/api";

export const LoginContext = createContext();

export const LoginProvider = ({ children }) => {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const recoveredUser = JSON.parse(localStorage.getItem("userLAB"));

    if (recoveredUser) {
      // Use a função de API apropriada para obter o usuário
      obterUsuario(recoveredUser._id)
        .then((data) => {
          setUser(data);
          setLoading(false);
        })
        .catch((error) => {
          console.error("Erro ao obter usuário:", error);
          setLoading(false);
        });
    } else {
      setLoading(false);
    }
  }, []);

  // Outras funções permanecem as mesmas, mas use as funções apropriadas do api.js

  const loginCreateAccount = async (loginUser) => {
    // Use a função de API apropriada para cadastrar o usuário
    try {
      const user = await cadastrarUsuario(loginUser);
      setUser(user);
      setLoading(false);
      navigate("/");
    } catch (error) {
      console.error("Erro ao cadastrar usuário:", error);
    }
  };

  const login = async (formData) => {
    // Use a função de API apropriada para fazer login
    try {
      const data = await login(formData.email, formData.password);
      localStorage.setItem("userLAB", JSON.stringify(data.usuario));
      setUser(data.usuario);
      setLoading(false);
      navigate("/");
    } catch (error) {
      console.error("Erro ao fazer login:", error);
    }
  };

  const logout = () => {
    // Use a função de API apropriada para fazer logout
    deslogarUsuario();
    localStorage.removeItem("userLAB");
    setUser(null);
    navigate("/login");
  };

  // Atualize as funções updateUser e outras conforme necessário

  return (
    <LoginContext.Provider
      value={{
        authenticated: !!user,
        user,
        loading,
        login,
        logout,
        loginCreateAccount,
      }}
    >
      {children}
    </LoginContext.Provider>
  );
};
