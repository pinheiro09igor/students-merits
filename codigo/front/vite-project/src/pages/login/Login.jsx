//* Components
import Input from "../../components/inputs/Input";
import Form from "../../components/forms/Form";
// import Button from "../../components/buttons/Button";
import { useFormik } from "formik";
import { useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { LoginService } from "../../hooks";

import {
  Box,
  Button,
  Container,
  CssBaseline,
  FormControl,
  FormControlLabel,
  Radio,
  RadioGroup,
  TextField,
  ThemeProvider,
  Typography,
  createTheme,
} from "@mui/material";

import { Alert, AlertTitle } from "@mui/material";

//* React
import { useState, useContext } from "react";

//* Context
import { LoginContext } from "../../context/LoginContext";

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
  console.log("Login");

  const formik = useFormik({
    initialValues: {
      email: "",
      senha: "",
      tipo: "",
    },
    validationSchema: Yup.object({
      email: Yup.string().required("Campo obrigatório"),
      senha: Yup.string().required("Campo obrigatório"),
      tipo: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        console.log("values");
        const token = await LoginService.login(
          values.email,
          values.senha,
          values.tipo
        );
        localStorage.setItem("token", token);
        navigate("/");
      } catch (error) {
        console.error(error);
      }
    },
  });

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
            {JSON.stringify(formik.values)}
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
        <Form
          onSubmit={(e) => {
            formik.handleSubmit(e);
          }}
          // onSubmit={handleSubmit}
        >
          <Input
            type="email"
            name="email"
            id="email"
            label="Email"
            onChange={formik.handleChange}
            required
          />
          <Input
            type="password"
            name="senha"
            id="senha"
            label="Senha"
            onChange={formik.handleChange}
            required
          />

          <FormControl>
            <RadioGroup
              row
              aria-labelledby="demo-row-radio-buttons-group-label"
              name="tipo"
              onChange={formik.handleChange}
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
            <Button type="submit" className="submit" id="submit">
              Entrar
            </Button>
          </div>
        </Form>
      </section>
    </div>
  );
};

export default Login;
