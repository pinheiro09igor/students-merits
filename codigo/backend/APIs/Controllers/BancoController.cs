using APIs.Modelos.Dtos;
using APIs.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BancoController : ControllerBase
{
    private readonly IBancoRepositorio _repositorio;
    public BancoController(IBancoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    /// <summary>
    /// Este Endpoint é resposável por realizar uma transferência bancária.
    /// </summary>
    /// <param name="dto">Dados enviados no corpo da requisição.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("transferir")]
    public async Task<IActionResult> Transferir(RealizarTransferenciaDto dto)
    {
        var resposta = await _repositorio.Transferir(dto);
        if (!resposta) return BadRequest();
        return Ok(new
        {
            status = StatusCodes.Status200OK.ToString(),
            mensagem = "Transferência realizada com sucesso"
        });
    }

    /// <summary>
    /// Este Endpoint é responsável por gerar um extrato bancário.
    /// </summary>
    /// <param name="identificador">CPF ou CNPJ do usuário.</param>
    /// <returns>IActionResult</returns>
    [HttpPost("extrato/{identificador}")]
    public async Task<IActionResult> ExtratoBancario(string identificador)
    {
        var extrato = await _repositorio.GerarExtrato(identificador);
        if (extrato is null) return BadRequest();
        return Ok(new
        {
            extrato
        });
    }
}