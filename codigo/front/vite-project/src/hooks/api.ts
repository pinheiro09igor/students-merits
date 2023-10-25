import axios from "axios";
import { Aluno, Empresa } from "../types/User";

const api = axios.create({
   baseURL: "https://localhost:7077",
});

export async function cadastrarUsuario(formData) {
   const nome = formData.get("name");
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

   const response = await api.post("/api/Alunos", requestBody);
   return response.data;
}

export async function cadastrarEmpresa(usuario: Empresa) {
   const response = await api.post("/api/Empresas", usuario);
   return response.data;
}

export async function obterTodosOsUsuarios() {
   const response = await api.get(`/api/Alunos`);
   return response.data;
}

export async function obterUsuario(credencial: string) {
   const response = await api.get(`/api/Alunos/${credencial}`);
   return response.data;
}

export async function obterTodasAsEmpresas() {
   const response = await api.get(`/api/Empresas`);
   return response.data;
}

export async function obterEmpresa(credencial: string) {
   const response = await api.get(`/api/Empresas/${credencial}`);
   return response.data;
}

export async function logar(email: string, senha: string, tipo: string) {
   const response = await api.post("/api/Auth/logar", {
      email,
      senha,
      tipo,
   });
   return {
      usuario: { email, senha, tipo },
      token: response.data.token,
   };
}

export async function apagarEmpresa(credencial: string) {
   await api.delete(`/api/Empresas/${credencial}`);
}

export async function apagarAluno(credencial: string) {
   await api.delete(`/api/Alunos/${credencial}`);
}

export async function atualizarEmpresa(
   credencial: string,
   dadosAtualizados: Empresa
) {
   const response = await api.put(
      `/api/Empresas/${credencial}`,
      dadosAtualizados
   );
   return response.data;
}

export async function atualizarAluno(
   credencial: string,
   dadosAtualizados: Aluno
) {
   const response = await api.put(
      `/api/Alunos/${credencial}`,
      dadosAtualizados
   );
   return response.data;
}

export async function deslogarUsuario() {
   localStorage.removeItem("token");
}
