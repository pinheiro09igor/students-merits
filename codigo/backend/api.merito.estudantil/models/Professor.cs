using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

[Table("Professores")]
public class Professor : Base
{
    [JsonProperty("nome")]
    public string Nome { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("senha")]
    public string Senha { get; set; }

    [Required]
    [JsonProperty("cpf")] 
    public string Cpf { get; set; }
    
    [JsonProperty("moedas")]
    public double Moedas { get; set; }
}