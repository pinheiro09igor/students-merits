using APIs.Interfaces;
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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var alunos = await _repositorio.ObterTodos();
        return Ok(alunos);
    }

    [HttpGet("{credencial}")]
    public async Task<IActionResult> Get([FromRoute] string credencial)
    {
        var aluno = await _repositorio.ObterPorCredencial(credencial);
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
    public async Task<IActionResult> Delete([FromRoute] string credencial)
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
