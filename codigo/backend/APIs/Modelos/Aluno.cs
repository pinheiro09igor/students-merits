using APIs.Modelos.Dtos;
using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Aluno : Usuario
{
    public string RG { get; set; }

    public string CPF { get; set; }

    public string EnderecoId { get; set; }

    public Endereco Endereco { get; set; }

    public string InstituicaoDeEnsino { get; set; }

    public Aluno()
    {
        
    }

    public Aluno(CadastrarAlunoDto dto)
    {
        Id = Guid.NewGuid().ToString();
        Nome = dto.Nome;
        Email = dto.Email;
        Senha = dto.Senha;
        RG = dto.RG;
        CPF = dto.CPF;
        EnderecoId = Id;
        Endereco = dto.Endereco;
        Endereco.EnderecoId = Id;
        InstituicaoDeEnsino = dto.InstituicaoDeEnsino;
    }
}
