using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Vantagem : Base
{
    [Required]
    [JsonProperty("nome")]
    public string Nome { get; set; }

    [Required]
    [JsonProperty("descricao")]
    public string Descricao { get; set; }

    [Required]
    [JsonProperty("valor")]
    public double Valor { get; set; }

    [Required]
    [JsonProperty("fotoName")]
    public string FotoName { get; set; }
    
    [Required]
    [JsonProperty("foto")]
    public string Foto { get; set; }
    
    [Required]
    [JsonProperty("idEmpresa")]
    public string IdEmpresa { get; set; }

    [JsonProperty("resgatadaPor")]
    public string ResgatadaPor { get; set; }
}