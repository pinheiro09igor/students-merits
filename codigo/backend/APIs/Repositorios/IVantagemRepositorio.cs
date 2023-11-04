using APIs.Modelos.Dtos;
using APIs.Modelos.Entidade;

namespace APIs.Repositorios;

public interface IVantagemRepositorio
{
    Task<ICollection<Vantagem>> ObterTodos(int quantidadeDeItens);
    Task<Vantagem> ObterPorNome(string nome);
    Task<Vantagem> Adicionar(CadastrarVantagemDto dto);
    Task<Vantagem> Atualizar(string nome, AtualizarVantagemDto dto);
    Task<int> Apagar(string nome);
    Task<Vantagem> ObterVantagemDeUmAluno(string identificadorAluno, string nomeVantagem="");
    Task<Vantagem> TrocarMoedas(string identificadorAluno, string nomeVantagem);
}