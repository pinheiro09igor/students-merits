using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Login
{
    [Required]
    [JsonProperty("email")]
    public string Email { get; set; }

    [Required]
    [JsonProperty("senha")]
    public string Senha { get; set; }
}