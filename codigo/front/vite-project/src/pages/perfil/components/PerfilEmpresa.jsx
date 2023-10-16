/* eslint-disable react/prop-types */
import Form from "../../../components/forms/Form";
import Input from "../../../components/inputs/Input";
import Button from "../../../components/buttons/Button";

const PerfilEmpresa = ({ user, onSubmit, deleteAccount }) => {
  return (
    <div className="flex flex-column row-gap-10rem max-width-50rem">
      <section className="title-section">
        <div className="title">
          <h1 className="title">
            Meus dados<span>.</span>
          </h1>
        </div>
      </section>

      <Form onSubmit={onSubmit}>
        <div className="edit-login flex flex-column row-gap-1rem max-width-50rem">
          <Input
            type="text"
            name="nome"
            id="name"
            initialValue={user.pessoa.nome}
            label="Nome da Empresa"
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
    </div>
  );
};

export default PerfilEmpresa;
