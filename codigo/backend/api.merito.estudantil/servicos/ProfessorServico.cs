using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class ProfessorServico : IRepositorioGenerico<Professor>, IRepositorioTransferenciaDeMoedas
{
    private readonly Contexto _contexto;
    private readonly IEmail _email;
    private readonly IRepositorioGenerico<Aluno> _repositorioAluno;
    private readonly IRepositorioTransacao _repositorioTransacao;
    public ProfessorServico(
        Contexto contexto, 
        IEmail email,
        IRepositorioGenerico<Aluno> repositorioAluno, 
        IRepositorioTransacao repositorioTransacao)
    {
        _contexto = contexto;
        _email = email;
        _repositorioAluno = repositorioAluno;
        _repositorioTransacao = repositorioTransacao;
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

    public async Task TransferirMoedas(Transferencia transferencia)
    {
        await using var transacaoEntityFrameworkCore = await _contexto.Database.BeginTransactionAsync();
        
        try
        {
            var aluno = await _repositorioAluno.ObterPorCredencial(transferencia.IdentificadorAluno);
            var professor = await ObterPorCredencial(transferencia.IdentificadorProfessor);

            if (aluno is null || professor is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            if (professor.Moedas < transferencia.Moedas)
                throw new Exception(StatusCodes.Status400BadRequest.ToString());

            professor.Moedas -= transferencia.Moedas;
            aluno.Moedas += transferencia.Moedas;

            await _repositorioAluno.Atualizar(aluno.Id, aluno);
            await Atualizar(professor.Id, professor);

            await _repositorioTransacao.CriarTransacao(new Transacao()
            {
                Id = Guid.NewGuid().ToString(),
                DestinatarioIdentificador = aluno.Id,
                RemetenteIdentificador = professor.Id,
                VantagemIdentificador = "",
                Valor = transferencia.Moedas,
                Data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                Descricao = transferencia.Descricao,
            });
            await _email.EnviarEmail(new Email()
            {
                EmailDestino = aluno.Email,
                Assunto = "Moedas recebidas!",
                Mensagem = $"Você recebeu {transferencia.Moedas} moedas do professor {professor.Nome}"
            });
        }
        catch
        {
            await transacaoEntityFrameworkCore.RollbackAsync();
        }
    }
}