/* eslint-disable react/prop-types */
import Form from "../../../components/forms/Form";
import Input from "../../../components/inputs/Input";
import Select from "../../../components/selects/Select";
import Button from "../../../components/buttons/Button";
import LinkedButton from "../../../components/linked-buttons/LinkedButton";

const PerfilProfessor = ({
  user,
  cursos,
  instituicoes,
  instituicao,
  onChange,
  onSubmit,
  departamentos,
  deleteAccount,
}) => {
  return (
    <div className="flex flex-column row-gap-10rem">
      <section className="description-section">
        <div className="ponts">
          <span>moedas</span>
          <h1 className="ponts">{user.pessoa.carteira.saldo}</h1>
        </div>

        <div className="description">
          <h3 className="title-description">
            Distribua <span>moedas</span> para seus alunos
          </h3>
          <p className="description">Distribua moedas aos seus alunos como forma de reconhecimento por bom comportamento, participação em aula, etcs.
          </p>

          <div className="hud-btn">
            <LinkedButton
              type="button"
              id="btn-extrato"
              className="black"
              to="/extrato"
            >
              Ver extrato
            </LinkedButton>
          </div>
        </div>
      </section>

      <section className={`form-section-${user.pessoa.tipo}`}>
        <Form onSubmit={onSubmit}>
          <div className="inputs-form">
            <div className="edit-login">
              <Input
                type="text"
                name="nome"
                id="name"
                initialValue={user.pessoa.nome}
                label="Nome completo"
                disabled
              />
              <Input
                type="email"
                name="email"
                id="email"
                initialValue={user.pessoa.email}
                label="Email"
                required
              />
              <Input
                type="password"
                name="senha"
                id="senha"
                initialValue={user.pessoa.senha}
                label="Senha"
                required
              />
            </div>

            <div className="infos-aluno">
              <div className="cpf-rg">
                <Input
                  type="text"
                  name="cpf"
                  id="cpf"
                  initialValue={user.cpf}
                  label="CPF"
                  disabled
                />
                <Input
                  type="text"
                  name="rg"
                  id="rg"
                  initialValue={user.rg}
                  label="RG"
                  disabled
                />
              </div>

              <Select
                name="departamento"
                id="departamento"
                label="Departamento"
                initialValue={user.departamento}
                options={departamentos}
                required
              />

              <div className="instituicao-curso flex column-gap-2rem">
                <Select
                  name="instituicaoEnsino"
                  id="instituicaoEnsino"
                  label="Instituição"
                  initialValue={instituicao._id}
                  options={instituicoes}
                  onChange={onChange}
                  required
                />
                <Select
                  name="curso"
                  id="curso"
                  label="Curso"
                  options={cursos}
                  initialValue={user.curso}
                  required
                />
              </div>
            </div>
          </div>

          <div className="button-submit">
            <Button
              type="button"
              className="delete"
              id="delete"
              onClick={deleteAccount}
            >
              Apagar Perfil
            </Button>
            <Button type="submit" className="submit" id="submit">
              Editar
            </Button>
          </div>
        </Form>
      </section>
    </div>
  );
};

export default PerfilProfessor;
