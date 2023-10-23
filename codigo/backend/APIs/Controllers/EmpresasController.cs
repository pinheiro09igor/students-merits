using APIs.Interfaces;
using APIs.Modelos.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaRepositorio _repositorio;
    public EmpresasController(IEmpresaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var empresas = await _repositorio.ObterTodos();
        return Ok(empresas);
    }

    [HttpGet("{credential}")]
    public async Task<IActionResult> Get([FromRoute] string credential)
    {
        var empresa = await _repositorio.ObterPorCredencial(credential);
        if (empresa is null) return NotFound(new
        {
            status = StatusCodes.Status404NotFound.ToString(),
            message = "EMPRESA NÃO ENCONTRADA"
        });
        return Ok(empresa);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CadastrarEmpresaDto dto)
    {
        var empresa = await _repositorio.Criar(dto);
        if (!empresa) return BadRequest(new
        {
            status = StatusCodes.Status400BadRequest.ToString(),
            message = "ERRO AO CADASTRAR"
        });
        return Created($"https://localhost:7077/api/Empresas/{dto.Email}", new
        {
            status = StatusCodes.Status201Created.ToString(),
            message = "CADASTRADO COM SUCESSO"
        });
    }

    [HttpPut("{credencial}")]
    public async Task<IActionResult> Put([FromRoute] string credencial, [FromBody] AtualizarEmpresaDto dto)
    {
        var empresa = await _repositorio.Atualizar(credencial, dto);
        if (!empresa) return NotFound(new
        {
            status = StatusCodes.Status404NotFound.ToString(),
            message = "EMPRESA NÃO ENCONTRADA"
        });
        return NoContent();
    }

    [HttpDelete("{credencial}")]
    public async Task<IActionResult> Delete([FromRoute] string credencial)
    {
        var empresa = await _repositorio.Apagar(credencial);
        if (!empresa) return NotFound(new
        {
            status = StatusCodes.Status404NotFound.ToString(),
            message = "EMPRESA NÃO ENCONTRADA"
        });
        return NoContent();
    }
}
