using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IRepositorioTransacao
{
    Task<ICollection<Transacao>> ObterTransacaoPeloIdentificadorDoRemetenteOuIdentificadorDoDestinatario(string credencial);
    Task<Transacao> CriarTransacao(Transacao transacao);
}
