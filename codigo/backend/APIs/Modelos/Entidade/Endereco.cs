using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Entidade;

public class Endereco
{
    [Required]
    [Key]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Rua { get; set; } = string.Empty;
    
    [Required]
    public string Numero { get; set; } = string.Empty;
    
    [Required]
    public string Bairro { get; set; } = string.Empty;
    
    [Required]
    public string Cidade { get; set; } = string.Empty;
    
    [Required]
    public string Estado { get; set; } = string.Empty;
    
    [Required]
    public string Cep { get; set; } = string.Empty;
    
    // Entidades relacionadas
    public string UsuarioId { get; set; }
    public Usuario UsuarioQuePertenceAEsseEndereco { get; set; }
}
