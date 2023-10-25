import { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";

import { LoginContext } from "../../context/LoginContext";

import "./Header.css";

const Header = () => {
   const { user, logout } = useContext(LoginContext);
   const [tipoUser, setTipoUser] = useState(null);

   useEffect(() => {
      if (user) {
         setTipoUser(user.tipo);
      } else return;
   }, [user]);

   return (
      <nav className="header">
         <ul className="list-itens">
            <div className="section-1">
               <li>
                  <Link to="/">
                     <img
                        className="logo"
                        src="/logo.png"
                        alt="students-merits"
                     />
                  </Link>
               </li>

               {user && (
                  <div className="itens-header">
                     {tipoUser === "Aluno" && (
                        <>
                           <li>
                              <Link to="/">Ver Vantagens</Link>
                           </li>
                           <li>
                              <Link to="/extrato">Ver extrato</Link>
                           </li>
                        </>
                     )}

                     {tipoUser === "Professor" && (
                        <>
                           <li>
                              <Link to="/extrato">Ver extrato</Link>
                           </li>
                           <li>
                              <Link to="/">Distribuir moedas</Link>
                           </li>
                        </>
                     )}

                     {tipoUser === "Empresa" && (
                        <>
                           <li>
                              <Link to="/">Ver Vantagens</Link>
                           </li>
                           <li>
                              <Link to="/cadastroVantagens">
                                 Cadastrar Vantagens
                              </Link>
                           </li>
                        </>
                     )}
                  </div>
               )}
            </div>

            <div className="section-2">
               <div className="subsection-2">
                  {user ? (
                     <>
                        <li>
                           <Link to="/login" onClick={logout}>
                              Logout
                           </Link>
                        </li>
                        <li>
                           <Link to="/perfil">Perfil</Link>
                        </li>
                     </>
                  ) : (
                     <>
                        <li>
                           <Link to="/cadastrar">Cadastrar</Link>
                        </li>
                        <li>
                           <Link to="/login">Entrar</Link>
                        </li>
                     </>
                  )}
               </div>
            </div>
         </ul>
      </nav>
   );
};

export default Header;
