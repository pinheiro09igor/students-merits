using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Endereco
{
    [Key]
    [Required]
    public string? EnderecoId { get; set; }

    [MinLength(3), MaxLength(255)]
    public string? Rua { get; set; }

    [Required]
    public int Numero { get; set; }

    [Required]
    [MaxLength(255)]
    public string? Bairro { get; set; }

    [Required]
    [MaxLength(255)]
    public string? Cidade { get; set; }

    [Required]
    [MinLength(8), MaxLength(8)]
    public string? CEP { get; set; }
}
