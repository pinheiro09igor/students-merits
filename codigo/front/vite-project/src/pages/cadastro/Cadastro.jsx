//* Components
import Input from "../../components/inputs/Input";
import Select from "../../components/selects/Select";
import Form from "../../components/forms/Form";
import Button from "../../components/buttons/Button";

//* React
import { useEffect, useState, useContext } from "react";

//* Context
import { LoginContext } from "../../context/LoginContext";

//* CSS
import "./Cadastro.css";

const CadastroAluno = () => {
  const url = "http://localhost:7077/api/Aluno";

  //* states
  const [instituicoes, setInstituicoes] = useState([]);
  const [cursos, setCursos] = useState([]);
  const [userType, setUserType] = useState("aluno");

  const { loginCreateAccount } = useContext(LoginContext);

  useEffect(() => {
    fetch(`${url}/instituicaoEnsino`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        }

        throw response;
      })
      .then((data) => setInstituicoes(data));
  }, []);

  const handleSwitchUserType = ({ target }) => {
    setUserType(target.id);
  };

  const handleChangeCourse = (selectedId) => {
    if (selectedId !== "0") {
      const instituicaoCursos = instituicoes.find(
        (instituicao) => instituicao._id === selectedId
      );
      setCursos(instituicaoCursos.cursos);
    } else setCursos([]);
  };

  const handleSubmit = (formData) => {
    if (!formData) {
      return;
    }

    const objectForm = Object.fromEntries(formData);

    fetch(`${url}/${userType}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(objectForm),
    })
      .then((resp) => resp.json())
      .then((data) => {
        alert(data.msg);
        data.response.tipo = userType;

        loginCreateAccount(data.response);
      })
      .catch((err) => {
        console.error(err);
        alert("Não foi possível criar a conta.");
      });
  };

  return (
    <div className="cadastrar-body">
      <section className="title-section">
        <div className="title">
          <h5 className="subtitle">CADASTRE AQUI</h5>
          <h1 className="title">
            Crie uma nova conta<span>.</span>
          </h1>
          <p className="login">
            Já possui uma conta?{" "}
            <a href="/login" className="login-page">
              Login
            </a>
          </p>
        </div>
      </section>

      <section className="form-section">
        <div className="switch-user-type">
          <Button
            type="button"
            id="aluno"
            className="switch"
            onClick={handleSwitchUserType}
          >
            Aluno
          </Button>
          <Button
            type="button"
            id="empresa"
            className="switch"
            onClick={handleSwitchUserType}
          >
            Empresa
          </Button>
        </div>

        <Form className="form-register" onSubmit={handleSubmit}>
          {userType === "aluno" && (
            <>
              <Input
                type="text"
                name="nome"
                id="name"
                label="Nome completo"
                required
              />
              <Input
                type="email"
                name="email"
                id="email"
                label="Email"
                required
              />
              <Input
                type="password"
                name="senha"
                id="senha"
                label="Senha"
                required
              />

              <div className="cpf-rg">
                <Input type="text" name="cpf" id="cpf" label="CPF" required />
                <Input type="text" name="rg" id="rg" label="RG" required />
              </div>

              <Input
                type="text"
                name="endereco"
                id="endereco"
                label="Endereço"
                required
              />
              <div className="instituicao-curso">
                <Select
                  name="instituicaoEnsino"
                  id="instituicaoEnsino"
                  label="Instituição"
                  options={instituicoes}
                  onChange={handleChangeCourse}
                  required
                />
                <Select
                  name="curso"
                  id="curso"
                  label="Curso"
                  options={cursos}
                  required
                />
              </div>
            </>
          )}

          {userType === "empresa" && (
            <>
              <Input
                type="text"
                name="nome"
                id="name"
                label="Nome da Empresa"
                required
              />
              <Input
                type="email"
                name="email"
                id="email"
                label="Email"
                required
              />
              <Input
                type="password"
                name="senha"
                id="senha"
                label="Senha"
                required
              />
            </>
          )}
          <div className="button-submit">
            <Button type="submit" className="submit" id="submit">
              Cadastrar
            </Button>
          </div>
        </Form>
      </section>
    </div>
  );
};

export default CadastroAluno;
