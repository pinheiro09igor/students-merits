import { http } from "../../../utils";
import { IVantagem } from "./interfaces";

export class VantagemService {
  static async getAllVantagens(id?: string): Promise<IVantagem[]> {
    const loggedInUserId = localStorage.getItem("id");
    const body = id ? { id: loggedInUserId } : undefined;

    const response = await http.post("/vantagem", body);
    return response.data;
  }

  static async getVantagemById(id: string): Promise<IVantagem> {
    const response = await http.get(`/vantagem/${id}`);
    return response.data;
  }

  static async createVantagem(vantagem: IVantagem): Promise<void> {
    await http.post("/vantagem", vantagem);
  }

  static async updateVantagem(vantagem: IVantagem): Promise<void> {
    await http.put(`/vantagem/${vantagem._id}`, vantagem);
  }

  static async deleteVantagem(id: string): Promise<void> {
    await http.delete(`/vantagem/${id}`);
  }

  static async resgatarVantagem(id: string): Promise<void> {
    const loggedInUserId = localStorage.getItem("id");
    const body = { id: id, idAluno: loggedInUserId };

    await http.post("/aluno/resgatarVantagem", body);
  }
}
