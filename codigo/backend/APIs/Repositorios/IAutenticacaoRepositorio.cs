using APIs.Modelos.Dtos;

namespace APIs.Repositorios;

public interface IAutenticacaoRepositorio
{
    Task<string> Logar(LoginDto dto);
    public bool ValidarToken(string token);
}
