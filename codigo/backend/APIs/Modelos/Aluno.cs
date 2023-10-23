using APIs.Modelos.Dtos;
using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Aluno
{
    [Key]
    public string Id { get; set; }

    [Required]
    [MinLength(3), MaxLength(255)]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6), MaxLength(20)]
    public string Senha { get; set; }

    [Required]
    [MinLength(8), MaxLength(8)]
    public string RG { get; set; }

    [Required]
    [MinLength(11), MaxLength(11)]
    public string CPF { get; set; }

    [Required]
    public Endereco Endereco { get; set; }

    [Required]
    [MinLength(2), MaxLength(255)]
    public string InstituicaoDeEnsino { get; set; }

    /// <summary>
    /// Construtor padrão da classe.
    /// </summary>
    public Aluno()
    {
        
    }

    /// <summary>
    /// Construtor respónsável por preencher as propriedades da classe 
    /// 'Aluno' com os dados enviados no corpo da requisição.
    /// </summary>
    public Aluno(CadastrarAlunoDto dto)
    {
        Id = Guid.NewGuid().ToString();
        dto.Endereco.Id = Guid.NewGuid().ToString();
        dto.Endereco.AlunoRef = Id;
        Nome = dto.Nome;
        Email = dto.Email;
        Senha = dto.Senha;
        RG = dto.RG;
        CPF = dto.CPF;
        Endereco = dto.Endereco;
        InstituicaoDeEnsino = dto.InstituicaoDeEnsino;
        Endereco.Aluno = this;
    }
}
