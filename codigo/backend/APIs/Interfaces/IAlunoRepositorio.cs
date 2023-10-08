using APIs.Modelos.Dtos;

namespace APIs.Interfaces;

public interface IAlunoRepositorio
{
    Task<bool> Criar(CadastrarAlunoDto dto);
    Task<bool> Atualizar(string credencial, AtualizarAlunoDto dto);
    Task<List<MostrarAlunoDto>> ObterTodos();
    Task<MostrarAlunoDto> ObterPorCredencial(string credencial);
    Task<bool> Apagar(string credencial);
    Task<string> Logar(LoginDto dto);
}
