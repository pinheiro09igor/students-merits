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

        return empresa is null ? throw new Exception(StatusCodes.Status404NotFound.ToString()) : empresa;
    }

    public async Task<Empresa> Criar(Empresa entidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();

        try
        {
            entidade.Id = Guid.NewGuid().ToString();
            await _contexto.Empresas.AddAsync(entidade);
            await _contexto.Usuarios.AddAsync(new Usuario()
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                Email = entidade.Email,
                Senha = entidade.Senha,
                Tipo = "empresa"
            });

            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();

            if (resposta == 0) throw new Exception(StatusCodes.Status400BadRequest.ToString());
            return entidade;
        } catch
        {
            await transacao.RollbackAsync();
            return null;
        }
    }

    public async Task<Empresa> Atualizar(string credencial, Empresa novaEntidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();

        try
        {
            var empresaEncontrada = await ObterPorCredencial(credencial) ?? throw new Exception(StatusCodes.Status404NotFound.ToString());
            var usuarioEncontrado = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Id.Equals(empresaEncontrada.Id) || u.Email.Equals(empresaEncontrada.Email));

            if (empresaEncontrada is null || usuarioEncontrado is null)
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            empresaEncontrada.Nome = novaEntidade.Nome;
            empresaEncontrada.Email = novaEntidade.Email;
            empresaEncontrada.Senha = novaEntidade.Senha;

            usuarioEncontrado.Nome = empresaEncontrada.Nome;
            usuarioEncontrado.Email = empresaEncontrada.Email;
            usuarioEncontrado.Senha = empresaEncontrada.Senha;

            _contexto.Empresas.Update(empresaEncontrada);
            _contexto.Usuarios.Update(usuarioEncontrado);

            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();

            if (resposta == 0) throw new Exception(StatusCodes.Status400BadRequest.ToString());
            return empresaEncontrada;
        } catch
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
            var empresaEncontrada = await ObterPorCredencial(credencial);
            var usuarioEncontrado = await _contexto.Usuarios.FirstOrDefaultAsync(
                u => u.Id.Equals(empresaEncontrada.Id) ||
                     u.Email.Equals(empresaEncontrada.Email));

            if (empresaEncontrada is null || usuarioEncontrado is null)
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            _contexto.Empresas.Remove(empresaEncontrada);
            _contexto.Usuarios.Remove(usuarioEncontrado);

            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();

            if (resposta == 0)
                throw new Exception(StatusCodes.Status400BadRequest.ToString());
        }
        catch
        {
            await transacao.RollbackAsync();
        }
    }
}