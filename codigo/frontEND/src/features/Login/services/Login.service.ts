import { http } from "../../../utils";

interface ILoginResponse {
  id: string;
  tipo: string;
}

export class LoginService {
  static async login(email: string, senha: string): Promise<ILoginResponse> {
    const response = await http.post("/login", { email, senha });
    return response.data;
  }

  static async signUp(
    nome: string,
    email: string,
    senha: string,
    identificador: string,
    tipo: string
  ): Promise<ILoginResponse> {
    const response = await http.post("/usuario", {
      nome: nome,
      senha: senha,
      email: email,
      identificador: identificador,
      tipoDeUsuario: tipo,
      enderecoDoUsuario: {
        rua: "",
        numero: "",
        bairro: "",
        cidade: "",
        estado: "",
        cep: "",
      },
    });
    return response.data;
  }
}
