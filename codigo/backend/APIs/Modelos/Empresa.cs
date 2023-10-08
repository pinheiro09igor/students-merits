using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Empresa : Usuario
{
    [MinLength(14), MaxLength(14)]
    public string CNPJ { get; set; }
}
