using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class AlunoServico : IRepositorioGenerico<Aluno>
{
    private readonly Contexto _contexto;
    
    public AlunoServico(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<ICollection<Aluno>> ObterTodos()
    {
        return await _contexto.Alunos.ToListAsync();
    }

    public async Task<Aluno> ObterPorCredencial(string credencial)
    {
        var aluno = await _contexto.Alunos
            .FirstOrDefaultAsync(
                a => a.Id.Equals(credencial) || 
                     a.Cpf.Equals(credencial) || 
                     a.Rg.Equals(credencial) || 
                     a.Email.Equals(credencial));

        if (aluno is null) throw new Exception(StatusCodes.Status404NotFound.ToString());
        return aluno;
    }

    public async Task<Aluno> Criar(Aluno entidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();

        try
        {
            entidade.Id = Guid.NewGuid().ToString();

            await _contexto.Alunos.AddAsync(entidade);
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

    public async Task<Aluno> Atualizar(string credencial, Aluno novaEntidade)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();
        
        try
        {
            var alunoEncontrado = await ObterPorCredencial(credencial);
            var usuarioEncontrado = await _contexto.Usuarios.FirstOrDefaultAsync(
                u => u.Id.Equals(alunoEncontrado.Id) || 
                     u.Email.Equals(alunoEncontrado.Email));
            
            if(alunoEncontrado is null || usuarioEncontrado is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            alunoEncontrado.Nome = novaEntidade.Nome;
            alunoEncontrado.Email = novaEntidade.Email;
            alunoEncontrado.Senha = novaEntidade.Senha;
            alunoEncontrado.Cpf = novaEntidade.Cpf;
            alunoEncontrado.Rg = novaEntidade.Rg;
            alunoEncontrado.Moedas = novaEntidade.Moedas;
            alunoEncontrado.Endereco = novaEntidade.Endereco;

            usuarioEncontrado.Nome = alunoEncontrado.Nome;
            usuarioEncontrado.Email = alunoEncontrado.Email;
            usuarioEncontrado.Senha = alunoEncontrado.Senha;

            _contexto.Alunos.Update(alunoEncontrado);
            _contexto.Usuarios.Update(usuarioEncontrado);
            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();
            
            if(resposta == 0) throw new Exception(StatusCodes.Status400BadRequest.ToString());
            return alunoEncontrado;
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
            var alunoEncontrado = await ObterPorCredencial(credencial);
            var usuarioEncontrado = await _contexto.Usuarios.FirstOrDefaultAsync(
                u => u.Id.Equals(alunoEncontrado.Id) || 
                     u.Email.Equals(alunoEncontrado.Email));
        
            if(alunoEncontrado is null || usuarioEncontrado is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            _contexto.Alunos.Remove(alunoEncontrado);
            _contexto.Usuarios.Remove(usuarioEncontrado);
            var resposta = await _contexto.SaveChangesAsync();

            await transacao.CommitAsync();
            if(resposta == 0) throw new Exception(StatusCodes.Status400BadRequest.ToString());
        }
        catch
        {
            await transacao.RollbackAsync();
        }
    }
}