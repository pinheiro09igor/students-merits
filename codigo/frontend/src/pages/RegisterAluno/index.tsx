import { ThemeProvider } from "@emotion/react";
import {
   Typography,
   createTheme,
   Container,
   CssBaseline,
   Box,
   Grid,
   TextField,
   Button,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { cadastrarUsuario } from "../../hooks/api";
import { Aluno, Endereco } from "../../types/User";

// TODO remove, this demo shouldn't need to reset the theme.
const defaultTheme = createTheme();

export default function RegisterAluno() {
  const navigate = useNavigate();

   const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();
      const data = new FormData(event.currentTarget);
      const nome = String(data.get("nome"));
      const email = String(data.get("email"));
      const senha = String(data.get("senha"));
      const rg = String(data.get("rg"));
      const cpf = String(data.get("cpf"));
      const instituicaoDeEnsino = String(data.get("instituicaoDeEnsino"));

      if(!nome) return;
      if(!email) return;
      if(!senha) return;
      if(!rg) return;
      if(!cpf) return;
      if(!instituicaoDeEnsino) return;

      const endereco: Endereco = {
        rua: "",
        numero: -1,
        bairro: "",
        cidade: "",
        cep: ""
      }

      const aluno: Aluno = {
          nome,
          email,
          senha,
          rg,
          cpf,
          endereco,
          instituicaoDeEnsino,
      };

      await cadastrarUsuario(aluno);
      navigate("/logar");
   };

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
                  Faça seu registro
               </Typography>
               <Box
                  component="form"
                  noValidate
                  onSubmit={handleSubmit}
                  sx={{ mt: 3 }}
               >
                  <Grid container spacing={2}>
                     <Grid item xs={12} sm={12}>
                        <TextField
                           autoComplete="given-name"
                           name="nome"
                           required
                           fullWidth
                           type="text"
                           id="nome"
                           label="Nome Completo"
                           autoFocus
                        />
                     </Grid>
                     <Grid item xs={12}>
                        <TextField
                           required
                           fullWidth
                           id="email"
                           type="email"
                           label="Email"
                           name="email"
                           autoComplete="email"
                        />
                     </Grid>
                     <Grid item xs={12}>
                        <TextField
                           required
                           fullWidth
                           name="senha"
                           label="Senha"
                           type="password"
                           id="senha"
                           autoComplete="senha"
                        />
                     </Grid>
                     <Grid item xs={12}>
                        <TextField
                           required
                           fullWidth
                           name="rg"
                           label="RG"
                           type="number"
                           id="RG"
                           autoComplete="rg"
                        />
                     </Grid>
                     <Grid item xs={12}>
                        <TextField
                           required
                           fullWidth
                           name="cpf"
                           label="CPF"
                           type="number"
                           id="cpf"
                           autoComplete="cpf"
                        />
                     </Grid>
                     <Grid item xs={12}>
                        <TextField
                           required
                           fullWidth
                           name="instituicaoDeEnsino"
                           label="Instituição de Ensino"
                           type="text"
                           id="instituicaoDeEnsino"
                           autoComplete="instituicaoDeEnsino"
                        />
                     </Grid>
                  </Grid>
                  <Button
                     type="submit"
                     fullWidth
                     variant="contained"
                     sx={{ mt: 3, mb: 2 }}
                  >
                     Cadastrar
                  </Button>
               </Box>
            </Box>
         </Container>
      </ThemeProvider>
   );
}
