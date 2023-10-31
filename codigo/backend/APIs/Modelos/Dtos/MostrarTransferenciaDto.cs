using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Dtos;

public class MostrarTransferenciaDto
{
    [Required]
    public string NomeUsuarioOrigem { get; set; }

    [Required]
    public string IdentificadorUsuarioOrigem { get; set; }
    
    [Required]
    public string NomeUsuarioDestino { get; set; }
    
    [Required]
    public string IdentificadorContaDestino { get; set; }

    [Required]
    public double Valor { get; set; }

    [Required] 
    public string DataTransferencia { get; set; } = string.Empty;
}