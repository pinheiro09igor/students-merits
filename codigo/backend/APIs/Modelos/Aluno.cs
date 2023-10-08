using APIs.Modelos.Dtos;
using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Aluno : Usuario
{
    [MinLength(10), MaxLength(10)]
    public string? RG { get; set; }

    [MinLength(11), MaxLength(11)]
    public string? CPF { get; set; }

    [Required]
    public string? EnderecoId { get; set; }

    [Required]
    public Endereco? Endereco { get; set; }

    [Required]
    public string? InstituicaoDeEnsino { get; set; }

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
        EnderecoId = dto.Endereco.EnderecoId;
        Endereco = dto.Endereco;
        InstituicaoDeEnsino = dto.InstituicaoDeEnsino;
    }
}
