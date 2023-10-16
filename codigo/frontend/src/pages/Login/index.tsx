import {
  Autocomplete,
  Box,
  Button,
  Container,
  CssBaseline,
  TextField,
  ThemeProvider,
  Typography,
  createTheme,
} from "@mui/material";
// import { SyntheticEvent, useContext, useState } from "react";
// import { AuthContext } from "../../context/auth/authContext";
import { useNavigate } from "react-router-dom";
import { LoginService } from "../../hooks";
import { useFormik } from "formik";
import * as Yup from "yup";

export default function Login() {
  const navigate = useNavigate();
  const options = ["Aluno", "Empresa"];

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

  const defaultTheme = createTheme();

  // export default function Login() {
  //   const auth = useContext(AuthContext);
  //   const navigate = useNavigate();

  //   const [tipo, setTipo] = useState("");

  //   const options = ["Aluno", "Empresa"];

  // const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
  //   event.preventDefault();
  //   const data = new FormData(event.currentTarget);
  //   const emailUser = String(data.get("email"));
  //   const userPassword = String(data.get("password"));
  //   const isLogged = await auth.logar(emailUser, userPassword, tipo);
  //   if (isLogged) {
  //     navigate("/dashboard");
  //   } else {
  //     navigate("/logar");
  //     alert("Email ou senha inválidos!");
  //   }
  // };

  // const handleTipo = (
  //   _e: SyntheticEvent<Element, Event>,
  //   value: string | ""
  // ) => {
  //   setTipo(value);
  // };

  // ---------- Return da função Login ----------

  return (
    <ThemeProvider theme={defaultTheme}>
      <Container component="main" maxWidth="xs">
        <CssBaseline />
        <Box
          sx={{
            marginTop: 8,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <Typography component="h1" variant="h5">
            Entrar
          </Typography>
          <Box
            component="form"
            onSubmit={(e) => {
              formik.handleSubmit(e);
            }}
            noValidate
            sx={{ mt: 1 }}
          >
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="Email"
              name="email"
              autoComplete="email"
              autoFocus
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Senha"
              type="password"
              id="password"
              autoComplete="current-password"
            />
            <Autocomplete
              onChange={formik.handleChange}
              disablePortal
              id="combo-box-demo"
              options={options}
              sx={{ width: 400, marginTop: 2 }}
              renderInput={(params) => <TextField {...params} label="Tipo" />}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Acessar
            </Button>
          </Box>
        </Box>
      </Container>
    </ThemeProvider>
  );
}
