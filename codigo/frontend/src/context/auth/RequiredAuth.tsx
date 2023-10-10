import Login from "../../pages/Login";

export const RequiredAuth = ({ children }: { children: JSX.Element }) => {
   const auth = localStorage.getItem('token');

   if(!auth) {
      return <Login/>;
   }
   return children;
};