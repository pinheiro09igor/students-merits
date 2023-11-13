using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class ObterVantagensDto
{
    [JsonProperty("id")]
    public string Id { get; set; }
}