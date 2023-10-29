using APIs.Modelos.Entidade;

namespace APIs.Modelos.Dtos;

public class MostrarUsuario
{
    public string Nome { get; set; }                         
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Identificador { get; set; }
    public string TipoDeUsuario { get; set; }
    public MostrarEndereco EnderecoDoUsuario { get; set; }

    public MostrarUsuario(Usuario usuario)
    {
        Nome = usuario.Nome;
        Email = usuario.Email;
        Senha = usuario.Senha;
        Identificador = usuario.Identificador;
        TipoDeUsuario = usuario.TipoDeUsuario;
        EnderecoDoUsuario = new MostrarEndereco(usuario.EnderecoDoUsuario);
    }
}