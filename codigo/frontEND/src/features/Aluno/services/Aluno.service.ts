import { http } from "../../../utils";
import { IAluno } from "./interfaces";

export class AlunoService {
  static async getAllAlunos(): Promise<IAluno[]> {
    const response = await http.get("/usuario");
    return response.data;
  }

  static async getAlunoById(id: string): Promise<IAluno> {
    const response = await http.get(`/usuario/${id}`);
    return response.data;
  }

  static async createAluno(aluno: IAluno): Promise<void> {
    await http.post("/usuario", aluno);
  }

  static async updateAluno(aluno: IAluno): Promise<void> {
    await http.put(`/usuario/${aluno._id}`, aluno);
  }

  static async deleteAluno(id: string): Promise<void> {
    await http.delete(`/usuario/${id}`);
  }
}
