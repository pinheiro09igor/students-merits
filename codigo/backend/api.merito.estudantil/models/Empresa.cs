using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.merito.estudantil.models;

[Table("Empresas")]
public class Empresa : Base
{
    [JsonProperty("nome")]
    public string Nome { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("senha")]
    public string Senha { get; set; }
}
