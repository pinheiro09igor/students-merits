using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Aluno: Usuario
{
    [Required]
    [JsonProperty("cpf")]
    public string Cpf { get; set; }

    [Required]
    [JsonProperty("rg")]
    public string Rg { get; set; }

    [Required]
    [JsonProperty("endereco")]
    public string Endereco { get; set; }
    
    [JsonProperty("moedas")]
    public double Moedas { get; set; }
}