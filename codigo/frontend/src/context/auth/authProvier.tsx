import { useEffect, useState } from "react";
import { Aluno, Empresa, Usuario } from "../../types/User";
import { AuthContext } from "./authContext";
import { deslogarUsuario, logarEmpresa, logarUsuario, obterUsuario } from "../../hooks/api";

export const AuthProvider = ({ children }: { children: JSX.Element }) => {
   const [usuario, setUser] = useState<Usuario | null>(null);
   const [aluno, setAluno] = useState<Aluno | null>(null);
   const [empresa, setEmpresa] = useState<Empresa | null>(null);

   useEffect(() => {
      const validateToken = async () => {
         const token = localStorage.getItem('token');
         const email = localStorage.getItem('email');
         if(token && email) {
            const data = await obterUsuario(email);
            if(data.usuario && data.token) {
               setUser(data.usuario);
               setAluno(data.usuario);
            }
         }
      };
      validateToken();
   }, []);

   const logar = async (email: string, senha: string, tipo: string) => {
      console.log(tipo);
      if(tipo == "Aluno") {
         const loginData = await logarUsuario(email, senha);
         if(loginData.token) {
            console.log(loginData.token);
            const userData = await obterUsuario(email);
            setUser(loginData.usuario);
            setAluno(userData.data);
            setToken(loginData.token, email);
            return true;
         }
         return false;
      }
      if(tipo == "Empresa") {
         const data = await logarEmpresa(email, senha);
         if(data.token) {
            const empresa = await obterUsuario(email);
            setUser(data.usuario);
            setEmpresa(empresa.data);
            setToken(data.token, email);
            return true;
         }
         return false;
      }
      return false;
   }

   const deslogar = () => {
      deslogarUsuario();
      setUser(null);
   }

   const setToken = (token: string, email: string) => {
      localStorage.setItem('token', token);
      localStorage.setItem('email', email);
   }

   // ---------- Return da função AuthProvider ----------

   return (
      <AuthContext.Provider value={{ usuario, aluno, empresa, logar, deslogar }}>
         {children}
      </AuthContext.Provider>
   );
}