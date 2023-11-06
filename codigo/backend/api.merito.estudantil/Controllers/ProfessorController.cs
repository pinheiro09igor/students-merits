using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.AspNetCore.Mvc;

namespace api.merito.estudantil.Controllers;

[Route("professor")]
[ApiController]
public class ProfessorController : ControllerBase
{
    private readonly IRepositorioGenerico<Professor> _repositorio;
    private readonly IRepositorioTransferenciaDeMoedas _repositorioTransferenciaDeMoedas;
    public ProfessorController(IRepositorioGenerico<Professor> repositorio, IRepositorioTransferenciaDeMoedas repositorioTransferenciaDeMoedas)
    {
        _repositorio = repositorio;
        _repositorioTransferenciaDeMoedas = repositorioTransferenciaDeMoedas;
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
                message = "PROFESSOR NÃO ENCONTRADO"
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Professor professor)
    {
        try
        {
            var professorCriado = await _repositorio.Criar(professor);
            return Created($"http://localhost:3000/professor/{professor.Cpf}", professorCriado);
        }
        catch
        {
            return BadRequest(new
            {
                statusCode = StatusCodes.Status400BadRequest.ToString(),
                message = "ERRO AO CADASTRAR PROFESSOR"
            });
        }
    }
    
    [HttpPut("{credencial}")]
    public async Task<IActionResult> Atualizar([FromRoute] string credencial, [FromBody] Professor professor)
    {
        try
        {
            await _repositorio.Atualizar(credencial, professor);
            return NoContent();
        }
        catch(Exception e)
        {
            if (e.Message.Equals(StatusCodes.Status400BadRequest.ToString()))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest.ToString(),
                    message = "ERRO AO CADASTRAR PROFESSOR"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "PROFESSOR NÃO ENCONTRADO"
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
                    message = "ERRO AO APAGAR PROFESSOR"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "PROFESSOR NÃO ENCONTRADO"
            });
        }
    }

    [HttpPost("transferirMoedas")]
    public async Task<IActionResult> TransferirMoedas([FromBody] Transferencia transferencia)
    {
        try
        {
            await _repositorioTransferenciaDeMoedas.TransferirMoedas(transferencia);
            return Ok();
        }
        catch(Exception e)
        {
            if (e.Message.Equals(StatusCodes.Status404NotFound.ToString()))
            {
                return NotFound(new
                {
                    statusCode = StatusCodes.Status404NotFound.ToString(),
                    message = "ALUNO OU PROFESSOR NÃO ENCONTRADO"
                });
            }
            return BadRequest(new
            {
                statusCode = StatusCodes.Status400BadRequest.ToString(),
                message = "ERRO AO TRANSFERIR MOEDAS"
            });
        }
    }
}