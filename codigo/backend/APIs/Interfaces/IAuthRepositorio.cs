using APIs.Modelos.Dtos;
using System.Security.Claims;

namespace APIs.Interfaces;

public interface IAuthRepositorio
{
    Task<string> Cadastrar(CadastrarEmpresaDto dto);
    Task<string> Cadastrar(CadastrarAlunoDto dto);
    Task<string> Logar(LoginDto dto);
    public ClaimsPrincipal ValidateToken(string token);
}
