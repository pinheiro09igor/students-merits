using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Usuario : Base
{
    [JsonProperty("nome")]
    public string Nome { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("senha")]
    public string Senha { get; set; }
}