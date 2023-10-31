using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Dtos;

public class RealizarTransferenciaDto
{
    [Required]
    public string IdentificadorContaOrigem { get; set; }
    
    [Required]
    public string IdentificadorContaDestino { get; set; }

    [Required]
    public double Valor { get; set; }
}