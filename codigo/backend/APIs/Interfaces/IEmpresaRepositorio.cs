using APIs.Modelos;
using APIs.Modelos.Dtos;

namespace APIs.Interfaces;

public interface IEmpresaRepositorio
{
    Task<Empresa> Criar(CadastrarEmpresaDto dto);
    Task<bool> Atualizar(string credencial, AtualizarEmpresaDto dto);
    Task<List<MostrarEmpresaDto>> ObterTodos();
    Task<MostrarEmpresaDto> ObterPorCredencial(string credencial);
    Task<bool> Apagar(string credencial);
}
