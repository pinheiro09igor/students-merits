using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Endereco
{
    [Key]
    [Required]
    public string? EnderecoId { get; set; }

    public string? Rua { get; set; }

    public int Numero { get; set; }

    public string? Bairro { get; set; }

    public string? Cidade { get; set; }

    public string? CEP { get; set; }
}
