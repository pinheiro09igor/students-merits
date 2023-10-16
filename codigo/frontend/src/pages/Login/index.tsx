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
// import { SyntheticEvent, useContext, useState } from "react";
// import { AuthContext } from "../../context/auth/authContext";
import { useNavigate } from "react-router-dom";
import { LoginService } from "../../hooks";
import { useFormik } from "formik";
import * as Yup from "yup";

// export default function Login() {
//   const auth = useContext(AuthContext);
//   const navigate = useNavigate();

//   const [tipo, setTipo] = useState("");

//   const options = ["Aluno", "Empresa"];

//   const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
//     event.preventDefault();
//     const data = new FormData(event.currentTarget);
//     const emailUser = String(data.get("email"));
//     const userPassword = String(data.get("password"));
//     const isLogged = await LoginService.login(
//       values.email,
//       values.senha,
//       values.tipo
//     );
//     if (isLogged) {
//       navigate("/dashboard");
//     } else {
//       navigate("/logar");
//       alert("Email ou senha inválidos!");
//     }
//   };

//   const handleTipo = (
//     _e: SyntheticEvent<Element, Event>,
//     value: string | ""
//   ) => {
//     setTipo(value);
//   };

export const Login = () => {
  const navigate = useNavigate();

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
      console.log("dentro funcao");
      try {
        console.log("dentro try");
        const token = await LoginService.login(
          values.email,
          values.senha,
          values.tipo
        );
        localStorage.setItem("token", token);
        navigate("/dashboard");
      } catch (error) {
        console.error(error);
        alert("Email ou senha inválidos!");
      }
    },
  });

  const defaultTheme = createTheme();

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
            {/* {JSON.stringify(formik.values)} */}
          </Typography>
          <form
            onSubmit={(e) => {
              e.preventDefault();
            }}
            noValidate
          >
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="Email"
              name="email"
              onChange={formik.handleChange}
              autoComplete="email"
              autoFocus
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="senha"
              label="Senha"
              type="password"
              id="senha"
              onChange={formik.handleChange}
              autoComplete="current-password"
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
            <Button
              onClick={() => {
                console.log("chamada funcao");
                formik.handleSubmit();
              }}
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Acessar
            </Button>
          </form>
        </Box>
      </Container>
    </ThemeProvider>
  );
};
