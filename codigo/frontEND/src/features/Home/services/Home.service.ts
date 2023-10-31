import { http } from '../../../utils';
import { ISaldo } from './interfaces';

export class HomeService {
  static async getSaldo(): Promise<ISaldo> {
    const id = localStorage.getItem('id');

    const response = await http.get('/saldo/' + id);

    return response.data;
  }

  static async postTransacao(professorId: string, alunoId: string, moedas: number, descricao: string): Promise<void> {
    const body = {
      professorId,
      alunoId,
      moedas,
      descricao,
    };

    await http.post(`/professor/transferirMoedas`, body);
  }
}
