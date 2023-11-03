using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Entidade;

public class VantagemDeCadaAluno
{
    [Required]
    [Key]
    public string Id { get; set; }
    
    [Required]
    public string IdentificadorAluno { get; set; }
    
    [Required]
    public string NomeDaVantagem { get; set; }
    
    [Required]
    public double ValorDaVantagem { get; set; }
    
    [Required]
    public string ObtidaEm { get; set; }
}