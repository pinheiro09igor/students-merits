using APIs.Interfaces;
using APIs.Modelos;
using APIs.Modelos.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlunosController : ControllerBase
{
    private readonly IAlunoRepositorio _repositorio;
    public AlunosController(IAlunoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _repositorio.Logar(dto);
        if (token is null) return NotFound(new
        {
            status = StatusCodes.Status404NotFound.ToString(),
            message = "ALUNO NÃO ENCONTRADO"
        });
        return Ok(new
        {
            token
        });
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var alunos = await _repositorio.ObterTodos();
        return Ok(alunos);
    }

    [HttpGet("{credential}")]
    public async Task<IActionResult> Get(string credential)
    {
        var aluno = await _repositorio.ObterPorCredencial(credential);
        if(aluno is null) return NotFound(new
        {
            status = StatusCodes.Status404NotFound.ToString(),
            message = "ALUNO NÃO ENCONTRADO"
        });
        return Ok(aluno);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CadastrarAlunoDto dto)
    {
        var aluno = await _repositorio.Criar(dto);
        if (!aluno) return BadRequest(new
        {
            status = StatusCodes.Status400BadRequest.ToString(),
            message = "ERRO AO CADASTRAR"
        });
        return Created($"https://localhost:7077/api/Alunos/{dto.Email}", new
        {
            status = StatusCodes.Status201Created.ToString(),
            message = "CADASTRADO COM SUCESSO"
        });
    }

    [HttpPut("{credencial}")]
    public async Task<IActionResult> Put([FromRoute] string credencial, [FromBody] AtualizarAlunoDto dto)
    {
        var aluno = await _repositorio.Atualizar(credencial, dto);
        if (!aluno) return NotFound(new
        {
            status = StatusCodes.Status404NotFound.ToString(),
            message = "ALUNO NÃO ENCONTRADO"
        });
        return NoContent();
    }

    [HttpDelete("{credencial}")]
    public async Task<IActionResult> Delete(string credencial)
    {
        var aluno = await _repositorio.Apagar(credencial);
        if (!aluno) return NotFound(new
        {
            status = StatusCodes.Status404NotFound.ToString(),
            message = "ALUNO NÃO ENCONTRADO"
        });
        return NoContent();
    }
}
