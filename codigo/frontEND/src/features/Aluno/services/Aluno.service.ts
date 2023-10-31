import { http } from '../../../utils';
import { IAluno } from './interfaces';

export class AlunoService {
  static async getAllAlunos(): Promise<IAluno[]> {
    const response = await http.get('/aluno');
    return response.data;
  }

  static async getAlunoById(id: string): Promise<IAluno> {
    const response = await http.get(`/aluno/${id}`);
    return response.data;
  }

  static async createAluno(aluno: IAluno): Promise<void> {
    await http.post('/aluno', aluno);
  }

  static async updateAluno(aluno: IAluno): Promise<void> {
    await http.put(`/aluno/${aluno._id}`, aluno);
  }

  static async deleteAluno(id: string): Promise<void> {
    await http.delete(`/aluno/${id}`);
  }
}