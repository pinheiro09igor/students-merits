using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.AspNetCore.Mvc;

namespace api.merito.estudantil.Controllers;

[Route("aluno")]
[ApiController]
public class AlunoController : ControllerBase
{
    private readonly IRepositorioGenerico<Aluno> _repositorio;
    private readonly IRepositorioResgateVantagem _repositorioResgateVantagem;

    public AlunoController(IRepositorioGenerico<Aluno> repositorio, IRepositorioResgateVantagem repositorioResgateVantagem)
    {
        _repositorio = repositorio;
        _repositorioResgateVantagem = repositorioResgateVantagem;
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
                message = "ALUNO NÃO ENCONTRADO"
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Aluno aluno)
    {
        try
        {
            var alunoCriado = await _repositorio.Criar(aluno);
            return Created($"http://localhost:3000/aluno/{aluno.Cpf}", alunoCriado);
        }
        catch
        {
            return BadRequest(new
            {
                statusCode = StatusCodes.Status400BadRequest.ToString(),
                message = "ERRO AO CADASTRAR ALUNO"
            });
        }
    }
    
    [HttpPut("{credencial}")]
    public async Task<IActionResult> Atualizar([FromRoute] string credencial, [FromBody] Aluno aluno)
    {
        try
        {
            await _repositorio.Atualizar(credencial, aluno);
            return NoContent();
        }
        catch(Exception e)
        {
            if (e.Message.Equals(StatusCodes.Status400BadRequest.ToString()))
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status400BadRequest.ToString(),
                    message = "ERRO AO CADASTRAR ALUNO"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "ALUNO NÃO ENCONTRADO"
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
                    message = "ERRO AO APAGAR ALUNO"
                });
            }
            return NotFound(new
            {
                statusCode = StatusCodes.Status404NotFound.ToString(),
                message = "ALUNO NÃO ENCONTRADO"
            });
        }
    }

    [HttpPost("resgatarVantagem")]
    public async Task<IActionResult> ResgatarVantagem([FromBody] ResgatarVantagem resgatarVantagem)
    {
        try
        {
            await _repositorioResgateVantagem.ResgatarVantagem(resgatarVantagem);
            return Ok();
        }
        catch(Exception e)
        {
            if(e.Message.Equals(StatusCodes.Status404NotFound.ToString()))
            {
                return NotFound(new
                {
                    statusCode = StatusCodes.Status404NotFound.ToString(),
                    message = "ALUNO OU VANTAGEM NÃO ENCONTRADO(A)"
                });
            }

            return BadRequest(new
            {
                statusCode = StatusCodes.Status400BadRequest.ToString(),
                message = "HOUVE UM ERRO DURANTE O RESGATE DA VANTAGEM"
            });
        }
    }
}