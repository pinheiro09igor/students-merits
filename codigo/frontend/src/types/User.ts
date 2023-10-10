export type Aluno = {
   nome: string
   email: string
   senha: string
   rg: string
   cpf: string
   endereco: Endereco
   instituicaoDeEnsino: string
}

export type Usuario = {
   email: string;
   password: string;
};

export type Endereco = {
   rua: string
   numero: number
   bairro: string
   cidade: string
   cep: string
}

export type Empresa = {
   nome: string
   email: string
   senha: string
   cnpj: string
}
