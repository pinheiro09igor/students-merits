using System.ComponentModel.DataAnnotations;
using APIs.Modelos.Enum;

namespace APIs.Modelos.Entidade;

/// <summary>
/// Esta classe representa qualquer tipo de usuário no sistema, podendo ser empresa,
/// professor e aluno, eles diferem de acordo com o valor do Enum: EnumTipoDeUsuario.
/// </summary>
public class Usuario
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Nome { get; set; } = string.Empty;
    
    [Required]                                          
    public string Email { get; set; } = string.Empty;   
    
    [Required]
    public string Senha { get; set; } = string.Empty;
    
    [Required]
    public string Identificador { get; set; } = string.Empty;

    [Required]
    public string TipoDeUsuario { get; set; }
    
    // Entidades Relacionadas
    [Required]
    public Endereco EnderecoDoUsuario { get; set; }
}
