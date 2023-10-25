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
import { useNavigate } from "react-router-dom";

const CadastroAluno = () => {
   const navigator = useNavigate();
   const url = "https://localhost:7077/api/Auth";

   //* states
   const [instituicoes, setInstituicoes] = useState([
      "ICEI PUC MINAS",
      "GOOGLE",
      "MICROSOFT",
   ]);
   const [cursos, setCursos] = useState([]);
   const [userType, setUserType] = useState("aluno");

   const { loginCreateAccount } = useContext(LoginContext);

   //  useEffect(() => {
   //     fetch(`${url}/instituicaoEnsino`, {
   //        method: "GET",
   //        headers: {
   //           "Content-Type": "application/json",
   //        },
   //     })
   //        .then((response) => {
   //           if (response.ok) {
   //              return response.json();
   //           }

   //           throw response;
   //        })
   //        .then((data) => setInstituicoes(data));
   //  }, []);

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

      const nome = formData.get("nome");
      const email = formData.get("email");
      const senha = formData.get("senha");
      const rg = formData.get("rg");
      const cpf = formData.get("cpf");
      const rua = formData.get("rua");
      const numero = formData.get("numero");
      const bairro = formData.get("bairro");
      const cidade = formData.get("cidade");
      const cep = formData.get("cep");
      const instituicaoEnsino = formData.get("instituicaoEnsino");

      const requestBody = {
         nome: nome,
         email: email,
         senha: senha,
         rg: rg,
         cpf: cpf,
         endereco: {
            rua: rua,
            numero: numero,
            bairro: bairro,
            cidade: cidade,
            cep: cep,
         },
         instituicaoDeEnsino: instituicaoEnsino,
      };

      console.log(requestBody);

      fetch(`${url}/${userType}`, {
         method: "POST",
         headers: {
            "Content-Type": "application/json",
         },
         body: JSON.stringify(requestBody),
      })
         .then((resp) => resp.json())
         .then((data) => {
            alert(data.status);
            if (data.status == 400) {
               alert(data.message);
            } else {
               navigator("/login");
            }
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
                        <Input
                           type="text"
                           name="cpf"
                           id="cpf"
                           label="CPF"
                           required
                        />
                        <Input
                           type="text"
                           name="rg"
                           id="rg"
                           label="RG"
                           required
                        />
                     </div>
                     <div className="endereco">
                        <Input
                           type="text"
                           name="rua"
                           id="rua"
                           label="Rua"
                           required
                        />
                        <Input
                           type="text"
                           name="numero"
                           id="numero"
                           label="Numero"
                           required
                        />
                        <Input
                           type="text"
                           name="bairro"
                           id="bairro"
                           label="Bairro"
                           required
                        />
                        <Input
                           type="text"
                           name="cidade"
                           id="cidade"
                           label="Cidade"
                           required
                        />
                        <Input
                           type="text"
                           name="cep"
                           id="cep"
                           label="CEP"
                           required
                        />
                     </div>
                     <div className="instituicao-curso">
                        <Select
                           name="instituicaoEnsino"
                           id="instituicaoEnsino"
                           label="Instituição"
                           options={instituicoes}
                           onChange={handleChangeCourse}
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
                        type="cnpj"
                        name="cnpj"
                        id="cnpj"
                        label="CNPJ"
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
