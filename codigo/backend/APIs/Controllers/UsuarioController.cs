using APIs.Modelos.Dtos;
using APIs.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepositorio _repositorio;
    public UsuarioController(IUsuarioRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    /// <summary>
    /// Este Endpoint obtem todos os usuários de acordo com um número específico de itens, caso uma quantidade
    /// não seja especificada, ele retornará por padrão 5 registros do banco de dados.
    /// </summary>
    /// <param name="quantidade">Quantidade de itens para ser retornado</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{quantidade:int}")]
    public async Task<IActionResult> ObterTodosOsClientes([FromRoute] int quantidade=5)
    {
        var usuariosDoBancoDeDados = await _repositorio.ObterTodos(quantidade);
        var usuarios = usuariosDoBancoDeDados
            .Select(usuario => new MostrarUsuario(usuario))
            .ToList();
        return Ok(usuarios);
    }

    /// <summary>
    /// Este Endpoint é responsável por obter um usuário de acordo com o identificador.
    /// </summary>
    /// <param name="identificador">CPF ou CNPJ do usuário.</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{identificador}")]
    public async Task<IActionResult> ObterUsuarioPorIdentificador([FromRoute] string identificador)
    {
        var usuario = await _repositorio.ObterPorIdentificador(identificador);
        if (usuario is null) return NotFound();
        return Ok(new MostrarUsuario(usuario));
    }

    /// <summary>
    /// Este Endpoint é responsável por cadastrar um usuário dos tipos: ALUNO, EMPRESA e PROFESSOR (todos os caracteres
    /// em letras MAIÚSCULAS). Além de cadastrar o usuário, ele cadastra o endereço do próprio.
    /// </summary>
    /// <param name="dto">Dados que serão enviados no corpo da requisição.</param>
    /// <returns>IActionResult</returns>
    [HttpPost]
    public async Task<IActionResult> AdicionarUsuario([FromBody] CadastrarUsuarioDto dto)
    {
        var resposta = await _repositorio.Adicionar(dto);
        if (resposta is null)
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                mensagem = "Erro ao cadastrar um usuário no banco de dados."
            });
        return Ok();
    }
    
    /// <summary>
    /// Este Endpoint é responsável por atualizar os dados de um usuário.
    /// </summary>
    /// <param name="identificador">CPF ou CNPJ do usuário.</param>
    /// <param name="dto">Dados que serão enviados no corpo da requisição.</param>
    /// <returns>IActionResult</returns>
    [HttpPut("{identificador}")]
    public async Task<IActionResult> AtualizarUsuario([FromRoute] string identificador, [FromBody] AtualizarUsuarioDto dto)
    {
        var resposta = await _repositorio.Atualizar(identificador, dto);
        if (resposta is null)
            return BadRequest(new
            {
                status = StatusCodes.Status400BadRequest.ToString(),
                mensagem = "Erro ao atualizar um usuário no banco de dados."
            });
        return NoContent();
    }

    /// <summary>
    /// Este Endpoint é responsável por apagar um usuário.
    /// </summary>
    /// <param name="identificador">CPF ou CNPJ do usuário.</param>
    /// <returns></returns>
    [HttpDelete("{identificador}")]
    public async Task<IActionResult> ApagarUsuario([FromRoute] string identificador)
    {
        var resposta = await _repositorio.Apagar(identificador);
        if (resposta == 0) return NotFound();
        return NoContent();
    }
}