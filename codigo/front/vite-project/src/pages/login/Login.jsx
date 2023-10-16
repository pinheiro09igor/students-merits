//* Components
import Input from "../../components/inputs/Input";
import Form from "../../components/forms/Form";
import Button from "../../components/buttons/Button";

import { Alert, AlertTitle } from "@mui/material";

//* React
import { useState, useContext } from "react";

//* Context
import { LoginContext } from "../../context/LoginContext";

//* CSS
import "./Login.css";

const Login = () => {
  const [error, setError] = useState(null);

  const { login } = useContext(LoginContext);

  const handleSubmit = async (formData) => {
    if (!formData) {
      return;
    }

    const resp = await login(formData);

    if (resp && resp.status !== 201) {
      setError(resp.msg);
    }
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
        <Form onSubmit={handleSubmit}>
          <Input type="email" name="email" id="email" label="Email" required />
          <Input
            type="password"
            name="senha"
            id="senha"
            label="Senha"
            required
          />

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
