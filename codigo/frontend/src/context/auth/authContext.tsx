import { createContext }  from 'react';
import { Aluno, Empresa, Usuario } from '../../types/User';

export type AuthContextType = {
   usuario: Usuario | null;
   aluno: Aluno | null;
   empresa: Empresa | null;
   logar: (email: string, senha: string, tipo: string) => Promise<boolean>;
   deslogar: () => void;
};

export const AuthContext = createContext<AuthContextType>(null!);
