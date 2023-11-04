import { useFormik } from "formik";
import * as Yup from "yup";
import { LoginService } from "../services";
import { useNavigate } from "react-router-dom";

export const Login = () => {
  const navigate = useNavigate();

  const formik = useFormik({
    initialValues: {
      email: "",
      senha: "",
    },
    validationSchema: Yup.object({
      email: Yup.string().email("Email inválido").required("Campo obrigatório"),
      senha: Yup.string().required("Campo obrigatório"),
    }),
    onSubmit: async (values) => {
      try {
        const data = await LoginService.login(values.email, values.senha);

        localStorage.setItem("id", data.id);
        localStorage.setItem("tipo", data.tipo);

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
            onSubmit={(e) => {
              formik.handleSubmit(e);
            }}
          >
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
            <div className="form-control mt-6">
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
            <div className="w-full flex justify-center">
              <a
                className="link link-primary"
                onClick={() => navigate("/signup")}
              >
                Cadastrar
              </a>
            </div>
            <div className="form-control mt-6">
              <input type="submit" value="Login" className="btn btn-primary" />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};
