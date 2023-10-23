using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIs.Modelos;

public class Endereco
{
    [Key]
    [JsonIgnore]
    public string Id { get; set; }

    [Required]
    [MinLength(2), MaxLength(255)]
    public string Rua { get; set; }

    [Required]
    public int Numero { get; set; }

    [Required]
    [MinLength(2), MaxLength(255)]
    public string Bairro { get; set; }

    [Required]
    [MinLength(2), MaxLength(255)]
    public string Cidade { get; set; }

    [Required]
    [MinLength(8), MaxLength(8)]
    public string CEP { get; set; }

    [JsonIgnore]
    public string AlunoRef { get; set; }
    [JsonIgnore]
    public Aluno Aluno { get; set; }
}
