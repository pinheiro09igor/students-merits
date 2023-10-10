import { ThemeProvider } from "@emotion/react";
import {
   createTheme,
   Container,
   CssBaseline,
   Box,
   Typography,
   Grid,
   TextField,
   Button,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { cadastrarEmpresa } from "../../hooks/api";
import { Empresa } from "../../types/User";

// TODO remove, this demo shouldn't need to reset the theme.
const defaultTheme = createTheme();

export default function RegistrarEmpresa() {
   const navigate = useNavigate();

   const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();
      const data = new FormData(event.currentTarget);
      let nome = String(data.get("nome"));
      let email = String(data.get("email"));
      let senha = String(data.get("senha"));
      let cnpj = String(data.get("cnpj"));

      if (!nome) return;
      if (!email) return;
      if (!senha) return;
      if (!cnpj) return;

      const empresa: Empresa = {
         nome,
         email,
         senha,
         cnpj,
      };

      nome = "";
      email = "";
      senha = "";
      cnpj = "";

      await cadastrarEmpresa(empresa);
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
                           name="cnpj"
                           label="CNPJ"
                           type="number"
                           id="cnpj"
                           autoComplete="cnpj"
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
