using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.AspNetCore.Mvc;

namespace api.merito.estudantil.Controllers;

[Route("")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IRepositorioUsuario _repositorio;
    public UsuarioController(IRepositorioUsuario repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        try
        {
            await _repositorio.Login(login);
            return Ok(new
            {
                statusCode = StatusCodes.Status200OK.ToString(),
                message = "USUÁRIO LOGADO COM SUCESSO"
            });
        }
        catch
        {
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "USUÁRIO NÃO ENCONTRADO"
            });
        }
    }

    [HttpGet("saldo/{credencial}")]
    public async Task<IActionResult> ObterSaldo([FromRoute] string credencial)
    {
        try
        {
            var saldo = await _repositorio.ObterSaldo(credencial);
            return Ok(saldo);
        }
        catch
        {
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "USUÁRIO NÃO ENCONTRADO"
            });
        }
    }
}