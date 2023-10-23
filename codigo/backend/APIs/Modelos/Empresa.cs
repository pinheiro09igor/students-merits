using APIs.Modelos.Dtos;
using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Empresa
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(14), MaxLength(14)]
    public string CNPJ { get; set; } = string.Empty;

    [Required]
    [MinLength(2), MaxLength(255)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6), MaxLength(20)]
    public string Senha { get; set; } = string.Empty;

    public Empresa()
    {
        
    }

    /// <summary>
    /// Construtor responsável por atribuir as propriedades da classe valores obtidos no corpo da requisição.
    /// </summary>
    /// <param name="dto">Dados enviados no corpo da requisição.</param>
    public Empresa(CadastrarEmpresaDto dto)
    {
        Id = Guid.NewGuid().ToString();
        Nome = dto.Nome;
        Senha = dto.Senha;
        Email = dto.Email;
        CNPJ = dto.CNPJ;
    }
}
