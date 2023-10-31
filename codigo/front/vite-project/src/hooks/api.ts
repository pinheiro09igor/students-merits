import axios from "axios";
import { Aluno, Empresa } from "../types/User";

const api = axios.create({
  baseURL: "https://localhost:7077",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

export async function cadastrarUsuario(usuario: Aluno) {
  const response = await api.post("/api/Alunos", usuario);
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

export async function logarUsuario(email: string, password: string) {
  const response = await api.post("/api/Auth/logar", {
    email,
    senha: password,
  });
  return response.data.token;
}

export async function logarEmpresa(email: string, password: string) {
  const response = await api.post("/api/Auth/logar", {
    email,
    senha: password,
  });
  return response.data.token;
}

export class LoginService {
  static async login(email: string, senha: string, tipo: string) {
    const response = await api.post("/api/Auth/logar", {
      email,
      senha,
      tipo,
    });
    return response.data.token;
  }
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
  const response = await api.put(`/api/Alunos/${credencial}`, dadosAtualizados);
  return response.data;
}

export async function deslogarUsuario() {
  localStorage.removeItem("token");
}
