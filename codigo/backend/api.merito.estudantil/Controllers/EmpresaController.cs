using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.AspNetCore.Mvc;

namespace api.merito.estudantil.Controllers;

[Route("empresa")]
[ApiController]
public class EmpresaController : ControllerBase
{
    private readonly IRepositorioGenerico<Empresa> _repositorio;
    public EmpresaController(IRepositorioGenerico<Empresa> repositorio)
    {
        _repositorio = repositorio;
    }
    
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        return Ok(await _repositorio.ObterTodos());
    }
    
    [HttpGet("{credencial}")]
    public async Task<IActionResult> ObterPorCredencial([FromRoute] string credencial)
    {
        try
        {
            var aluno = await _repositorio.ObterPorCredencial(credencial);
            return Ok(aluno);
        }
        catch
        {
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "EMPRESA NÃO ENCONTRADA"
            });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Empresa empresa)
    {
        try
        {
            var empresaCriada = await _repositorio.Criar(empresa);
            return Created($"http://localhost:3000/empresa/{empresa.Email}", empresaCriada);
        }
        catch
        {
            return BadRequest(new
            {
                statusCode = StatusCodes.Status400BadRequest.ToString(),
                message = "ERRO AO CADASTRAR EMPRESA"
            });
        }
    }
    
    [HttpPut("{credencial}")]
    public async Task<IActionResult> Atualizar([FromRoute] string credencial, [FromBody] Empresa empresa)
    {
        try
        {
            await _repositorio.Atualizar(credencial, empresa);
            return NoContent();
        }
        catch(Exception e)
        {
            if (e.Message.Equals(StatusCodes.Status400BadRequest.ToString()))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest.ToString(),
                    message = "ERRO AO CADASTRAR EMPRESA"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "EMPRESA NÃO ENCONTRADA"
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
                    message = "ERRO AO APAGAR EMPRESA"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "EMPRESA NÃO ENCONTRADA"
            });
        }
    }
}