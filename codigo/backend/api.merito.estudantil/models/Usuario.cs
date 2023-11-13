using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.merito.estudantil.models;

[Table("Usuarios")]
public class Usuario : Base
{
    [JsonProperty("nome")]
    public string Nome { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("senha")]
    public string Senha { get; set; }

    [JsonProperty("tipo")]
    public string Tipo { get; set; }
}