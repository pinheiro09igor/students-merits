using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Entidade;

public class Vantagem
{
    [Required]
    [Key]
    public string Id { get; set; }
    
    [Required]
    public string Nome { get; set; }
    
    [Required]
    public double Valor { get; set; }
    
    [Required]
    public string Descricao { get; set; }
    
    [Required]
    public string Foto { get; set; }
}