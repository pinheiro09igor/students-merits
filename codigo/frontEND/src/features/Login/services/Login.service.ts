import { http } from '../../../utils';

interface ILoginResponse {
  id: string;
  tipo: string;
}

export class LoginService {
  static async login(email: string, senha: string): Promise<ILoginResponse> {
    const response = await http.post('/login', { email, senha });
    return response.data;
  }
}
