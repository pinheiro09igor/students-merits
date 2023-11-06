using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IRepositorioVantagem
{
    Task<IEnumerable<Vantagem>> ObterTodos(ObterVantagensDto dto);
    Task<Vantagem> ObterPorCredencial(string credencial);
    Task<Vantagem> Criar(Vantagem entidade);
    Task<Vantagem> Atualizar(string credencial, Vantagem novaEntidade);
    Task Apagar(string credencial);
}