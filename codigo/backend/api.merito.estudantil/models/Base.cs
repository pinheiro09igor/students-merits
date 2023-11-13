using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Base
{
    [Key]
    [JsonProperty("_id")]
    public string Id { get; set; }
}