using APIs.Modelos.Dtos;
using APIs.Modelos.Entidade;

namespace APIs.Repositorios;

public interface IUsuarioRepositorio
{
    Task<ICollection<Usuario>> ObterTodos(int quantidadeDeItens);
    Task<Usuario> ObterPorIdentificador(string identificador);
    Task<Usuario> Adicionar(CadastrarUsuarioDto dto);
    Task<Usuario> Atualizar(string identificador, AtualizarUsuarioDto dto);
    Task<int> Apagar(string identificador);
}