using APIs.Modelos.Dtos;
using APIs.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VantagemController : ControllerBase
{
    private readonly IVantagemRepositorio _repositorio;
    public VantagemController(IVantagemRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    [HttpGet("{quantidade:int}")]
    public async Task<IActionResult> ObterTodas([FromRoute] int quantidade = 0)
    {
        var vantagens = await _repositorio.ObterTodos(quantidade);
        return Ok(vantagens);
    }

    [HttpGet("{nome}")]
    public async Task<IActionResult> ObterPorNome([FromRoute] string nome)
    {
        var vantagem = await _repositorio.ObterPorNome(nome);
        if (vantagem is null) return NotFound();
        return Ok(vantagem);
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarVantagem([FromBody] CadastrarVantagemDto dto)
    {
        var vantagem = await _repositorio.Adicionar(dto);
        if (vantagem is null) return BadRequest();
        return NoContent();
    }

    [HttpPut("{nome}")]
    public async Task<IActionResult> AtualizarVantagem([FromRoute] string nome, [FromBody] AtualizarVantagemDto dto)
    {
        var resposta = await _repositorio.Atualizar(nome, dto);
        if (resposta is null) return BadRequest();
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ApagarVantagem([FromRoute] string nome)
    {
        var resposta = await _repositorio.Apagar(nome);
        if (resposta == 0) return BadRequest();
        return NoContent();
    }

    [HttpGet("{identificadorAluno}/{nomeVantagem}")]
    public async Task<IActionResult> ObterVantagemDeUmAluno([FromRoute] string identificadorAluno,
        [FromRoute] string nomeVantagem)
    {
        var vantagem = await _repositorio.ObterVantagemDeUmAluno(identificadorAluno, nomeVantagem);
        if (vantagem is null) return BadRequest();
        return Ok(vantagem);
    }

    [HttpPost("{identificadorAluno}/{nomeVantagem}")]
    public async Task<IActionResult> TrocarMoedas([FromRoute] string identificadorAluno,
        [FromRoute] string nomeVantagem)
    {
        var resposta = await _repositorio.TrocarMoedas(identificadorAluno, nomeVantagem);
        if (resposta is null) return BadRequest();
        return NoContent();
    }
}