using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public abstract class Usuario
{
    public string Id { get; set; }

    [Required]
    [MinLength(3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6), MaxLength(12)]
    public string Senha { get; set; } = string.Empty;
}
