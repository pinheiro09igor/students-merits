using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class VantagemServico : IRepositorioVantagem
{
    private readonly Contexto _contexto;
    public VantagemServico(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<IEnumerable<Vantagem>> ObterTodos(ObterVantagensDto dto)
    {
        var vantagens = await _contexto.Vantagens.ToListAsync();
        return vantagens.Where(v => v.IdEmpresa.Equals(dto.EmpresaIdentificador));
    }

    public async Task<Vantagem> ObterPorCredencial(string credencial)
    {
        var vantagem = await _contexto.Vantagens
            .FirstOrDefaultAsync(
                a => a.Id.Equals(credencial));

        if (vantagem is null) throw new Exception(StatusCodes.Status404NotFound.ToString());
        return vantagem;
    }

    public async Task<Vantagem> Criar(Vantagem entidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();

        try
        {
            entidade.Id = Guid.NewGuid().ToString();

            await _contexto.Vantagens.AddAsync(entidade);
            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();

            if (resposta == 0) 
                throw new Exception(StatusCodes.Status400BadRequest.ToString());
            return entidade;
        }
        catch
        {
            await transacao.RollbackAsync();
            return null;
        }
    }

    public async Task<Vantagem> Atualizar(string credencial, Vantagem novaEntidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();
        
        try
        {
            var vantagemEncontrada = await ObterPorCredencial(credencial);
            
            if(vantagemEncontrada is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            vantagemEncontrada.Nome = novaEntidade.Nome;
            vantagemEncontrada.Descricao = novaEntidade.Descricao;
            vantagemEncontrada.Valor = novaEntidade.Valor;
            vantagemEncontrada.FotoName = novaEntidade.FotoName;
            vantagemEncontrada.Foto = novaEntidade.Foto;
            vantagemEncontrada.IdEmpresa = novaEntidade.IdEmpresa;

            _contexto.Vantagens.Update(vantagemEncontrada);
            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();
            
            if(resposta == 0) 
                throw new Exception(StatusCodes.Status400BadRequest.ToString());
            return vantagemEncontrada;
        }
        catch
        {
            await transacao.RollbackAsync();
            return null;
        }
    }

    public async Task Apagar(string credencial)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();

        try
        {
            var vantagemEncontrada = await ObterPorCredencial(credencial);
            if(vantagemEncontrada is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            _contexto.Vantagens.Remove(vantagemEncontrada);
            var resposta = await _contexto.SaveChangesAsync();

            await transacao.CommitAsync();
            if(resposta == 0) 
                throw new Exception(StatusCodes.Status400BadRequest.ToString());
        }
        catch
        {
            await transacao.RollbackAsync();
        }
    }
}