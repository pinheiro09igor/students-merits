using APIs.Modelos.Dtos;
using System.Security.Claims;

namespace APIs.Interfaces;

public interface IAuthRepositorio
{
    /// <summary>
    /// Cadastrar uma empresa no banco de dados.
    /// </summary>
    /// <param name="dto">Dados enviado no corpo da requisição.</param>
    /// <returns>Token de autenticação caso a empresa seja cadastrada com sucesso.</returns>
    Task<string> Cadastrar(CadastrarEmpresaDto dto);

    /// <summary>
    /// Cadastrar uma aluno no banco de dados.
    /// </summary>
    /// <param name="dto">Dados enviado no corpo da requisição.</param>
    /// <returns>Token de autenticação caso o aluno seja cadastrado com sucesso.</returns>
    Task<string> Cadastrar(CadastrarAlunoDto dto);

    /// <summary>
    /// Verifica se um aluno está cadastrado no banco de dados.
    /// </summary>
    /// <param name="dto">Dados enviado no corpo da requisição.</param>
    /// <returns>Token de autenticação caso os dados enviados no corpo da requisição estejam de acordo com os cadastrados no banco de dados.</returns>
    Task<string> Logar(LoginDto dto);

    /// <summary>
    /// Valida o token de autenticação enviados na rota da API com os dados do servidor que compõe o token.
    /// </summary>
    /// <param name="token">Token de autenticação.</param>
    /// <returns>Retorna um ClaimsPrincipal caso o token seja válido.</returns>
    public ClaimsPrincipal ValidateToken(string token);
}
