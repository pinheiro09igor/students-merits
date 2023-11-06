using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class EmpresaServico : IRepositorioGenerico<Empresa>
{
    private readonly Contexto _contexto;
    public EmpresaServico(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<ICollection<Empresa>> ObterTodos()
    {
        return await _contexto.Empresas.ToListAsync();
    }

    public async Task<Empresa> ObterPorCredencial(string credencial)
    {
        var empresa = await _contexto.Empresas
            .FirstOrDefaultAsync(a => a.Id.Equals(credencial) || a.Email.Equals(credencial));

        if (empresa is null) throw new Exception(StatusCodes.Status404NotFound.ToString());
        return empresa;
    }

    public async Task<Empresa> Criar(Empresa entidade)
    {
        entidade.Id = Guid.NewGuid().ToString();
        await _contexto.Empresas.AddAsync(entidade);
        var resposta = await _contexto.SaveChangesAsync();
        if (resposta == 0) throw new Exception(StatusCodes.Status400BadRequest.ToString());
        return entidade;
    }

    public async Task<Empresa> Atualizar(string credencial, Empresa novaEntidade)
    {
        var empresaEncontrada = await ObterPorCredencial(credencial);
        if(empresaEncontrada is null) throw new Exception(StatusCodes.Status404NotFound.ToString());

        empresaEncontrada.Nome = novaEntidade.Nome;
        empresaEncontrada.Email = novaEntidade.Email;
        empresaEncontrada.Senha = novaEntidade.Senha;

        _contexto.Empresas.Update(empresaEncontrada);
        var resposta = await _contexto.SaveChangesAsync();
        if(resposta == 0) throw new Exception(StatusCodes.Status400BadRequest.ToString());
        return empresaEncontrada;
    }

    public async Task Apagar(string credencial)
    {
        var empresaEncontrada = await ObterPorCredencial(credencial);
        if(empresaEncontrada is null) throw new Exception(StatusCodes.Status404NotFound.ToString());

        _contexto.Empresas.Remove(empresaEncontrada);
        var resposta = await _contexto.SaveChangesAsync();
        if(resposta == 0) throw new Exception(StatusCodes.Status400BadRequest.ToString());
    }
}