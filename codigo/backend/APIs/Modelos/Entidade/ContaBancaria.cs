using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Entidade;

public class ContaBancaria
{
    [Required]
    [Key]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Identificador { get; set; } = string.Empty;
    
    [Required]
    public double SaldoBancario { get; set; }
}
