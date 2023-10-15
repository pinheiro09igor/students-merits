using APIs.Interfaces;
using APIs.Modelos.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepositorio authRepositorio;
    public AuthController(IAuthRepositorio authRepositorio)
    {
        this.authRepositorio = authRepositorio;
    }

    [HttpGet("validar/{token}")]
    public IActionResult Validate([FromRoute] string token)
    {
        var claimsPrincipal = authRepositorio.ValidateToken(token);
        if (claimsPrincipal == null) return BadRequest(new
        {
            status = StatusCodes.Status400BadRequest.ToString(),
            message = "TOKEN NÃO É VÁLIDO"
        });
        return Ok(new
        {
            status = StatusCodes.Status200OK.ToString(),
            message = "TOKEN É VÁLIDO"
        });
    }

    [HttpPost("aluno")]
    public async Task<IActionResult> Criar([FromBody] CadastrarAlunoDto dto)
    {
        var token = await authRepositorio.Cadastrar(dto);
        if(token is null) return BadRequest(new
        {
            status = StatusCodes.Status400BadRequest.ToString(),
            message = "ERRO AO CADASTRAR ALUNO"
        });
        return Created($"https://localhost:7077/api/Alunos/{dto.Email}", new
        {
            token
        });
    }

    [HttpPost("empresa")]
    public async Task<IActionResult> Criar([FromBody] CadastrarEmpresaDto dto)
    {
        var token = await authRepositorio.Cadastrar(dto);
        if (token is null) return BadRequest(new
        {
            status = StatusCodes.Status400BadRequest.ToString(),
            message = "ERRO AO CADASTRAR EMPRESA"
        });
        return Created($"https://localhost:7077/api/Empresas/{dto.Email}", new
        {
            token
        });
    }

    [HttpPost("logar")]
    public async Task<IActionResult> Logar([FromBody] LoginDto dto)
    {
        var token = await authRepositorio.Logar(dto);
        if (token is null) return Unauthorized(new
        {
            status = StatusCodes.Status401Unauthorized.ToString(),
            message = "ERRO AO REALIZAR LOGIN"
        });
        return Ok(new
        {
            token
        });
    }
}
