using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIs.Modelos;

public class Endereco
{
    [Key]
    [Required]
    [JsonIgnore]
    public string? EnderecoId { get; set; } = Guid.NewGuid().ToString();

    public string? Rua { get; set; }

    public int Numero { get; set; }

    public string? Bairro { get; set; }

    public string? Cidade { get; set; }

    public string? CEP { get; set; }
}
