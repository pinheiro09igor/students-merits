using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Transferencia : Base
{
    [Required]
    [JsonProperty("professorId")]
    public string IdentificadorProfessor { get; set; }
    
    [Required]
    [JsonProperty("alunoId")]
    public string IdentificadorAluno { get; set; }

    [Required]
    [JsonProperty("moedas")]
    public double Moedas { get; set; }

    [JsonProperty("descricao")]
    public string Descricao { get; set; }
}