using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class AlunoServico : IRepositorioGenerico<Aluno>, IRepositorioResgateVantagem
{
    private readonly IRepositorioGenerico<Vantagem> _repositorioVantagem;
    private readonly IRepositorioGenerico<Transacao> _repositorioTransacao;
    private readonly IRepositorioGenerico<Empresa> _repositorioEmpresa;
    private readonly IEmail _email;
    private readonly Contexto _contexto;
    
    public AlunoServico(
        Contexto contexto, 
        IEmail email, 
        IRepositorioGenerico<Vantagem> repositorioVantagem, 
        IRepositorioGenerico<Transacao> repositorioTransacao, 
        IRepositorioGenerico<Empresa> repositorioEmpresa)
    {
        _contexto = contexto;
        _email = email;
        _repositorioVantagem = repositorioVantagem;
        _repositorioTransacao = repositorioTransacao;
        _repositorioEmpresa = repositorioEmpresa;
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

    public async Task ResgatarVantagem(ResgatarVantagem resgatarVantagem)
    {
        await using var transacaoEntityFrameworkCore = await _contexto.Database.BeginTransactionAsync();

        try
        {
            var aluno = await ObterPorCredencial(resgatarVantagem.IdentificadorAluno);
            if (aluno is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            var vantagem = await _repositorioVantagem.ObterPorCredencial(resgatarVantagem.IdentificadorVantagem);
            if(vantagem is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            if ((aluno.Moedas - vantagem.Valor) < 0) 
                throw new Exception(StatusCodes.Status400BadRequest.ToString());

            await Atualizar(aluno.Id, aluno);
            await _repositorioTransacao.Criar(new Transacao()
            {
                Id = Guid.NewGuid().ToString(),
                RemetenteIdentificador = aluno.Id,
                DestinatarioIdentificador = vantagem.IdEmpresa,
                Valor = vantagem.Valor,
                VantagemIdentificador = vantagem.Id,
                Data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                Descricao = $"Vantagem resgatada de nome: '{vantagem.Nome}'"
            });
            await _email.EnviarEmail(new Email()
            {
                EmailDestino = aluno.Email,
                Assunto = "Você resgatou uma vantagem!",
                Mensagem =
                    $"Você resgatou uma vantagem de nome '{vantagem.Nome}' por {vantagem.Valor} moedas. Código para resgate: {vantagem.Id}"
            });
            
            var empresa = await _repositorioEmpresa.ObterPorCredencial(vantagem.IdEmpresa);
            if (empresa is not null)
                await _email.EnviarEmail(new Email()
                {
                    EmailDestino = empresa.Email,
                    Assunto = "Uma vantagem foi resgatada!",
                    Mensagem = $"A vantagem de nome '{vantagem.Nome} foi resgatada pelo aluno '{aluno.Nome}' com sucesso!"
                });
        }
        catch
        {
            await transacaoEntityFrameworkCore.RollbackAsync();
        }
    }
}