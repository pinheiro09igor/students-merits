//* Components
import Input from "../../components/inputs/Input";
import Form from "../../components/forms/Form";
import { useNavigate } from "react-router-dom";
import { LoginService } from "../../hooks";

import {
   Button,
   FormControl,
   FormControlLabel,
   Radio,
   RadioGroup,
} from "@mui/material";

import { Alert, AlertTitle } from "@mui/material";

//* React
import { useState } from "react";

//* CSS
import "./Login.css";

// const Login = () => {
//   const [error, setError] = useState(null);

//   const { login } = useContext(LoginContext);

//   const handleSubmit = async (formData) => {
//     if (!formData) {
//       return;
//     }

//     const resp = await login(formData);

//     if (resp && resp.status !== 201) {
//       setError(resp.msg);
//     }
//   };

export const Login = () => {
   const navigate = useNavigate();
   const [error, setError] = useState(null);
   const [email, setEmail] = useState("");
   const [senha, setSenha] = useState("");
   const [tipo, setTipo] = useState("");
   console.log("Login");

   const handleSubmit = () => {
      console.log("handleSubmit");
      console.log(email);
      console.log(senha);
      console.log(tipo);
      LoginService.login(email, senha, tipo)
         .then((response) => {
            console.log(response);
            localStorage.setItem("token", response);
            navigate("/");
         })
         .catch((error) => {
            console.log(error);
            setError(error);
         });
   };

   return (
      <div className="login-body flex flex-column row-gap-5rem max-width-50rem">
         <section className="title-section">
            <div className="title">
               <h5 className="subtitle">BEM-VINDO(A) DE VOLTA</h5>
               <h1 className="title">
                  Login<span>.</span>
               </h1>
               <p className="login">
                  Faça login para ter acesso aos recursos da plataforma
               </p>
            </div>
         </section>

         <section className="form-section">
            {error && (
               <Alert severity="error">
                  <AlertTitle>Error</AlertTitle>
                  {error}
               </Alert>
            )}
            <Form>
               <Input
                  type="email"
                  name="email"
                  id="email"
                  label="Email"
                  onChange={(e) => setEmail(e.target.value)}
                  required
               />
               <Input
                  type="password"
                  name="senha"
                  id="senha"
                  label="Senha"
                  onChange={(e) => setSenha(e.target.value)}
                  required
               />

               <FormControl>
                  <RadioGroup
                     row
                     aria-labelledby="demo-row-radio-buttons-group-label"
                     name="tipo"
                     onChange={(e) => setTipo(e.target.value)}
                  >
                     <FormControlLabel
                        value="ALUNO"
                        control={<Radio />}
                        label="Aluno"
                     />
                     <FormControlLabel
                        value="EMPRESA"
                        control={<Radio />}
                        label="Empresa"
                     />
                  </RadioGroup>
               </FormControl>

               <div className="button-submit">
                  <Button
                     type="submit"
                     className="submit"
                     id="submit"
                     onChange={handleSubmit}
                  >
                     Entrar
                  </Button>
               </div>
            </Form>
         </section>
      </div>
   );
};

export default Login;
