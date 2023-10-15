using APIs.Modelos;
using APIs.Modelos.Dtos;

namespace APIs.Interfaces;

public interface IAlunoRepositorio
{
    Task<Aluno> Criar(CadastrarAlunoDto dto);
    Task<bool> Atualizar(string credencial, AtualizarAlunoDto dto);
    Task<List<MostrarAlunoDto>> ObterTodos();
    Task<MostrarAlunoDto> ObterPorCredencial(string credencial);
    Task<bool> Apagar(string credencial);
}
