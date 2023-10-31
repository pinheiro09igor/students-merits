using APIs.Modelos.Dtos;
using APIs.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenticacaoServico : ControllerBase
{
    private readonly IAutenticacaoRepositorio _repositorio;
    public AutenticacaoServico(IAutenticacaoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    /// <summary>
    /// Este Endpoint é responsável por buscar um usuário do banco de dados, caso ele exista,
    /// retorna o Token dele.
    /// </summary>
    /// <param name="dto">Dados enviados no corpo da requisição.</param>
    /// <returns>IActionResult</returns>
    [HttpPost]
    public async Task<IActionResult> Logar([FromBody] LoginDto dto)
    {
        var token = await _repositorio.Logar(dto);
        return Ok(new
        {
            token
        });
    }

    /// <summary>
    /// Este Endpoint é responsável por buscar um usuário do banco de dados, caso ele exista,
    /// retorna o Token dele.
    /// </summary>
    /// <param name="token">Token de autenticação do usuário.</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{token}")]
    public IActionResult ValidarToken([FromRoute] string token)
    {
        var valido = _repositorio.ValidarToken(token);
        return valido ? Ok(new
        {
            token
        }) : Unauthorized(new
        {
            token = (string)null
        });
    }
}