using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Professor : Usuario
{
    [Required]
    [JsonProperty("cpf")] 
    public string Cpf { get; set; }
    
    [JsonProperty("moedas")]
    public double Moedas { get; set; }
}