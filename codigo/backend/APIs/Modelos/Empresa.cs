using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos;

public class Empresa : Usuario
{
    public string CNPJ { get; set; }
}
