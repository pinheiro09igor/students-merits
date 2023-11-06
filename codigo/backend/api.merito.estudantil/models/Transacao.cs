using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Transacao
{
    [Key]
    [Required]
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [Required]
    [JsonProperty("destinatarioId")]
    public string DestinatarioIdentificador { get; set; }

    [Required]
    [JsonProperty("remetenteId")]
    public string RemetenteIdentificador { get; set; }

    [Required]
    [JsonProperty("valor")]
    public double Valor { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }
    
    [JsonProperty("descricao")]
    public string Descricao { get; set; }

    [JsonProperty("vantagemId")]
    public string VantagemIdentificador { get; set; }
}