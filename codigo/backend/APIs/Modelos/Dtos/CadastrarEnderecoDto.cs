using System.ComponentModel.DataAnnotations;

namespace APIs.Modelos.Dtos;

public class CadastrarEnderecoDto
{
    public string Rua { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
}