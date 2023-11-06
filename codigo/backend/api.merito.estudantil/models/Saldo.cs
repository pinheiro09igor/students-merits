using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.merito.estudantil.models;

public class Saldo
{
    [JsonProperty("moedas")]
    public double Moedas { get; set; }
    
    [JsonProperty("transacoes")]
    public ICollection<TransacaoComNome> Transacoes { get; set; }
}