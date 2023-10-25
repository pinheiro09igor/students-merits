import { useState, useEffect, createContext } from "react";
import { useNavigate } from "react-router-dom";
import {
   cadastrarUsuario,
   obterUsuario,
   deslogarUsuario,
   logar,
} from "../hooks/api";

export const LoginContext = createContext();

// eslint-disable-next-line react/prop-types
export const LoginProvider = ({ children }) => {
   const navigate = useNavigate();
   const [user, setUser] = useState(null);
   const [loading, setLoading] = useState(true);

   useEffect(() => {
      const recoveredUser = JSON.parse(localStorage.getItem("userLAB"));

      if (recoveredUser) {
         // Use a função de API apropriada para obter o usuário
         obterUsuario(recoveredUser.email)
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

   const loginCreateAccount = async (formData) => {
      // Use a função de API apropriada para cadastrar o usuário
      try {
         const user = await cadastrarUsuario(formData);
         setUser(user);
         setLoading(false);
         navigate("/");
      } catch (error) {
         console.error("Erro ao cadastrar usuário:", error);
      }
   };

   const login = async (formData) => {
      // Use a função de API apropriada para fazer login
      const email = formData.get("email");
      const senha = formData.get("senha");
      const tipo = formData.get("tipo");
      try {
         const data = await logar(email, senha, tipo);
         localStorage.setItem("userLAB", JSON.stringify(data.usuario));
         localStorage.setItem("token", JSON.stringify(data.token));
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
      localStorage.removeItem("token");
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
