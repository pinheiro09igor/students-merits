using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class TransacaoServico : IRepositorioTransacao
{
    private readonly Contexto _contexto;
    public TransacaoServico(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<ICollection<Transacao>> ObterTransacaoPeloIdentificadorDoRemetenteOuIdentificadorDoDestinatario(string credencial)
    {
        var transacoes = await _contexto.Transacoes.ToListAsync();
        if (transacoes.Count == 0) return new List<Transacao>();
        return transacoes.Where(transacao => 
            transacao.DestinatarioIdentificador.Equals(credencial) || 
            transacao.RemetenteIdentificador.Equals(credencial)).ToList();
    }

    public async Task<Transacao> CriarTransacao(Transacao transacao)
    {
        await using var transacaoEntityFrameworkCore = await _contexto.Database.BeginTransactionAsync();

        try
        {
            transacao.Id = Guid.NewGuid().ToString();
            transacao.Data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

            await _contexto.Transacoes.AddAsync(transacao);
            var resposta = await _contexto.SaveChangesAsync();
            await transacaoEntityFrameworkCore.CommitAsync();

            if (resposta == 0) 
                throw new Exception(StatusCodes.Status400BadRequest.ToString());
            return transacao;
        }
        catch
        {
            await transacaoEntityFrameworkCore.RollbackAsync();
            return null;
        }
    }
}