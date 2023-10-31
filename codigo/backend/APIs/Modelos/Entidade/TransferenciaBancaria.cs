using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Entidade;

public class TransferenciaBancaria
{
    [Required]
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string IdentificadorContaOrigem { get; set; }
    
    [Required]
    public string IdentificadorContaDestino { get; set; }

    [Required]
    public double Valor { get; set; }

    [Required] 
    public string DataTransferencia { get; set; } = string.Empty;
}