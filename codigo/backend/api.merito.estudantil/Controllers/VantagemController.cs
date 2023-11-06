using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.AspNetCore.Mvc;

namespace api.merito.estudantil.Controllers;

[Route("vantagem")]
[ApiController]
public class VantagemController : ControllerBase
{
    private readonly IRepositorioVantagem _repositorio;
    
    public VantagemController(IRepositorioVantagem repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpPost("all")]
    public async Task<IActionResult> ObterTodos(ObterVantagensDto dto)
    {
        return Ok(await _repositorio.ObterTodos(dto));
    }

    [HttpGet("{credencial}")]
    public async Task<IActionResult> ObterPorCredencial([FromRoute] string credencial)
    {
        try
        {
            var vantagem = await _repositorio.ObterPorCredencial(credencial);
            return Ok(vantagem);
        }
        catch
        {
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "VANTAGEM NÃO ENCONTRADA"
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Vantagem entidade)
    {
        try
        {
            var vantagemCriada = await _repositorio.Criar(entidade);
            return Created($"http://localhost:3000/vantagem/{vantagemCriada.Id}", vantagemCriada);
        }
        catch
        {
            return BadRequest(new
            {
                statusCode = StatusCodes.Status400BadRequest.ToString(),
                message = "ERRO AO CADASTRAR VANTAGEM"
            });
        }
    }
    
    [HttpPut("{credencial}")]
    public async Task<IActionResult> Atualizar([FromRoute] string credencial, [FromBody] Vantagem entidade)
    {
        try
        {
            await _repositorio.Atualizar(credencial, entidade);
            return NoContent();
        }
        catch(Exception e)
        {
            if (e.Message.Equals(StatusCodes.Status400BadRequest.ToString()))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest.ToString(),
                    message = "ERRO AO CADASTRAR VANTAGEM"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "VANTAGEM NÃO ENCONTRADA"
            });
        }
    }

    [HttpDelete("{credencial}")]
    public async Task<IActionResult> Apagar([FromRoute] string credencial)
    {
        try
        {
            await _repositorio.Apagar(credencial);
            return NoContent();
        }
        catch(Exception e)
        {
            if (e.Message.Equals(StatusCodes.Status400BadRequest.ToString()))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest.ToString(),
                    message = "ERRO AO APAGAR VANTAGEM"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "VANTAGEM NÃO ENCONTRADA"
            });
        }
    }
}