using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class ResgatarVantagem
{
    [JsonProperty("id")]
    public string IdentificadorVantagem { get; set; }

    [JsonProperty("idAluno")]
    public string IdentificadorAluno { get; set; }
}