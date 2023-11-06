using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IRepositorioGenerico<T>
{
    Task<ICollection<T>> ObterTodos();
    Task<T> ObterPorCredencial(string credencial);
    Task<T> Criar(T entidade);
    Task<T> Atualizar(string credencial, T novaEntidade);
    Task Apagar(string credencial);
}
