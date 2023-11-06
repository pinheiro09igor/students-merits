using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class ProfessorServico : IRepositorioGenerico<Professor>
{
    private readonly Contexto _contexto;
    
    public ProfessorServico(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<ICollection<Professor>> ObterTodos()
    {
        return await _contexto.Professores.ToListAsync();
    }

    public async Task<Professor> ObterPorCredencial(string credencial)
    {
        var professor = await _contexto.Professores
            .FirstOrDefaultAsync(
                a => a.Id.Equals(credencial) || 
                     a.Cpf.Equals(credencial) || 
                     a.Email.Equals(credencial));

        if (professor is null) 
            throw new Exception(StatusCodes.Status404NotFound.ToString());
        return professor;
    }

    public async Task<Professor> Criar(Professor entidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();

        try
        {
            entidade.Id = Guid.NewGuid().ToString();
            await _contexto.Professores.AddAsync(entidade);
            await _contexto.Usuarios.AddAsync(new Usuario()
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                Email = entidade.Email,
                Senha = entidade.Senha
            });
            
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

    public async Task<Professor> Atualizar(string credencial, Professor novaEntidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();
        
        try
        {
            var professorEncontrado = await ObterPorCredencial(credencial);
            var usuarioEncontrado = await _contexto.Usuarios.FirstOrDefaultAsync(
                u => u.Id.Equals(professorEncontrado.Id) || 
                     u.Email.Equals(professorEncontrado.Email));
        
            if(professorEncontrado is null || usuarioEncontrado is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            professorEncontrado.Nome = novaEntidade.Nome;
            professorEncontrado.Email = novaEntidade.Email;
            professorEncontrado.Senha = novaEntidade.Senha;
            professorEncontrado.Cpf = novaEntidade.Cpf;
            professorEncontrado.Moedas = novaEntidade.Moedas;

            usuarioEncontrado.Nome = professorEncontrado.Nome;
            usuarioEncontrado.Email = professorEncontrado.Email;
            usuarioEncontrado.Senha = professorEncontrado.Senha;

            _contexto.Professores.Update(professorEncontrado);
            _contexto.Usuarios.Update(usuarioEncontrado);
        
            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();
            
            if(resposta == 0) 
                throw new Exception(StatusCodes.Status400BadRequest.ToString());
            return professorEncontrado;
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
            var professorEncontrado = await ObterPorCredencial(credencial);
            var usuarioEncontrado = await _contexto.Usuarios.FirstOrDefaultAsync(
                u => u.Id.Equals(professorEncontrado.Id) || 
                     u.Email.Equals(professorEncontrado.Email));
        
            if(professorEncontrado is null || usuarioEncontrado is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            _contexto.Professores.Remove(professorEncontrado);
            _contexto.Usuarios.Remove(usuarioEncontrado);
            
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