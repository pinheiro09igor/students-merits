using System.ComponentModel.DataAnnotations;
using APIs.Modelos.Entidade;
using APIs.Modelos.Enum;

namespace APIs.Modelos.Dtos;

public class CadastrarUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Identificador { get; set; } = string.Empty;
    public string TipoDeUsuario { get; set; }
    public CadastrarEnderecoDto EnderecoDoUsuario { get; set; }
}