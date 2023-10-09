using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public abstract class Usuario
{
    public string Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
