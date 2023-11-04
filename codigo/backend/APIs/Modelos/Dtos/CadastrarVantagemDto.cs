using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Dtos;

public class CadastrarVantagemDto
{
    [Required]
    public string Nome { get; set; }
    
    [Required]
    public double Valor { get; set; }
    
    [Required]
    public string Descricao { get; set; }
    
    [Required]
    public string Foto { get; set; }
}