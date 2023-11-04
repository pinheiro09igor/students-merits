import { http } from "../../../utils";
import { IEmpresa } from "./interfaces";

export class EmpresaService {
  static async getAllEmpresas(): Promise<IEmpresa[]> {
    const response = await http.get("/usuario");
    return response.data;
  }

  static async getEmpresaById(id: string): Promise<IEmpresa> {
    const response = await http.get(`/usuario/${id}`);
    return response.data;
  }

  static async createEmpresa(empresa: IEmpresa): Promise<void> {
    await http.post("/usuario", empresa);
  }

  static async updateEmpresa(empresa: IEmpresa): Promise<void> {
    await http.put(`/usuario/${empresa._id}`, empresa);
  }

  static async deleteEmpresa(id: string): Promise<void> {
    await http.delete(`/usuario/${id}`);
  }
}
