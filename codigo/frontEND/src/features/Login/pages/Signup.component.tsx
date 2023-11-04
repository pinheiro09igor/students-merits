import { useFormik } from "formik";
import * as Yup from "yup";
import { LoginService } from "../services";
import { useNavigate } from "react-router-dom";

export const SignUp = () => {
  const navigate = useNavigate();

  const formik = useFormik({
    initialValues: {
      nome: "",
      email: "",
      senha: "",
      identificador: "",
      tipoDeUsuario: "ALUNO",
    },
    validationSchema: Yup.object({
      nome: Yup.string().required("Campo obrigatório"),
      email: Yup.string().email("Email inválido").required("Campo obrigatório"),
      senha: Yup.string().required("Campo obrigatório"),
      identificador: Yup.string().required("Campo obrigatório"),
      tipoDeUsuario: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        const data = await LoginService.signUp(
          values.nome,
          values.email,
          values.senha,
          values.identificador,
          values.tipoDeUsuario
        );

        navigate("/");
      } catch (error) {
        console.error(error);
      }
    },
  });

  return (
    <div
      className="flex w-full h-full justify-center items-center"
      style={{
        backgroundImage:
          "radial-gradient(rgba(255, 255, 255, 0.1) 1px, transparent 1px)",
        backgroundPosition: "50% 50%",
        backgroundSize: "1.1rem 1.1rem",
      }}
    >
      <div className="card bg-base-200 shadow-xl w-[400px]">
        <div className="card-body">
          <form
            className="flex flex-col justify-center gap-2"
            onSubmit={(e) => {
              formik.handleSubmit(e);
            }}
          >
            <div className="form-control">
              <input
                type="text"
                placeholder="Nome"
                id="nome"
                name="nome"
                className={`input input-bordered w-full ${
                  formik.errors.nome && formik.touched.nome && "input-error"
                }`}
                value={formik.values.nome}
                onChange={formik.handleChange}
              />
              {formik.errors.nome && formik.touched.nome && (
                <label className="label">
                  <span className="label-text-alt text-error">
                    {formik.errors.nome}
                  </span>
                </label>
              )}
            </div>
            <div className="form-control">
              <input
                type="text"
                placeholder="CPF/CNPJ"
                id="identificador"
                name="identificador"
                className={`input input-bordered w-full ${
                  formik.errors.identificador &&
                  formik.touched.identificador &&
                  "input-error"
                }`}
                value={formik.values.identificador}
                onChange={formik.handleChange}
              />
              {formik.errors.identificador && formik.touched.identificador && (
                <label className="label">
                  <span className="label-text-alt text-error">
                    {formik.errors.identificador}
                  </span>
                </label>
              )}
            </div>
            <div className="form-control">
              <input
                type="text"
                placeholder="Email"
                id="email"
                name="email"
                className={`input input-bordered w-full ${
                  formik.errors.email && formik.touched.email && "input-error"
                }`}
                value={formik.values.email}
                onChange={formik.handleChange}
              />
              {formik.errors.email && formik.touched.email && (
                <label className="label">
                  <span className="label-text-alt text-error">
                    {formik.errors.email}
                  </span>
                </label>
              )}
            </div>
            <div className="form-control">
              <input
                type="password"
                placeholder="Senha"
                id="senha"
                name="senha"
                className={`input input-bordered w-full ${
                  formik.errors.senha && formik.touched.senha && "input-error"
                }`}
                value={formik.values.senha}
                onChange={formik.handleChange}
              />
              {formik.errors.senha && formik.touched.senha && (
                <label className="label">
                  <span className="label-text-alt text-error">
                    {formik.errors.senha}
                  </span>
                </label>
              )}
            </div>
            <div className="form-control">
              <select
                className="select select-bordered w-full"
                id="tipoDeUsuario"
                name="tipoDeUsuario"
                value={formik.values.tipoDeUsuario}
                onChange={formik.handleChange}
              >
                <option value="ALUNO">Aluno</option>
                <option value="PROFESSOR">Professor</option>
                <option value="EMPRESA">Empresa</option>
              </select>
              {formik.errors.tipoDeUsuario && formik.touched.tipoDeUsuario && (
                <label className="label">
                  <span className="label-text-alt text-error">
                    {formik.errors.tipoDeUsuario}
                  </span>
                </label>
              )}
            </div>
            <div className="form-control mt-6">
              <input
                type="submit"
                value="Cadastrar"
                className="btn btn-primary"
              />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};
