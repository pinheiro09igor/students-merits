using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class TransacaoComNome
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("destinatarioId")]
    public string DestinatarioIdentificador { get; set; }

    [JsonProperty("destinatarioNome")]
    public string DestinatarioNome { get; set; }

    [JsonProperty("remetenteId")]
    public string RemetenteIdentificador { get; set; }
    
    [JsonProperty("remetenteNome")]
    public string RemetenteNome { get; set; }

    [JsonProperty("valor")]
    public double Valor { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("descricao")]
    public string Descricao { get; set; }

    [JsonProperty("vantagemId")]
    public string VantagemIdentificador { get; set; }
}