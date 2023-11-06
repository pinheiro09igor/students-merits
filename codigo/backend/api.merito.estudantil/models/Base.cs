using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Base
{
    [Key]
    [JsonProperty("id")]
    public string Id { get; set; }
}