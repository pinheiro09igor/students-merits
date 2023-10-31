import { http } from '../../../utils';
import { IEmpresa } from './interfaces';

export class EmpresaService {
  static async getAllEmpresas(): Promise<IEmpresa[]> {
    const response = await http.get('/empresa');
    return response.data;
  }

  static async getEmpresaById(id: string): Promise<IEmpresa> {
    const response = await http.get(`/empresa/${id}`);
    return response.data;
  }

  static async createEmpresa(empresa: IEmpresa): Promise<void> {
    await http.post('/empresa', empresa);
  }

  static async updateEmpresa(empresa: IEmpresa): Promise<void> {
    await http.put(`/empresa/${empresa._id}`, empresa);
  }

  static async deleteEmpresa(id: string): Promise<void> {
    await http.delete(`/empresa/${id}`);
  }
}