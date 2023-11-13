using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;

namespace api.merito.estudantil.servicos;

public class TransferirMoedasServico : IRepositorioTransferenciaDeMoedas
{
    private readonly Contexto _contexto;
    private readonly IEmail _email;
    private readonly IRepositorioGenerico<Aluno> _repositorioAluno;
    private readonly IRepositorioGenerico<Professor> _repositorioProfessor;
    private readonly IRepositorioTransacao _repositorioTransacao;
    
    public TransferirMoedasServico(Contexto contexto)
    {
        _contexto = contexto;
        _email = new EmailServico();
        _repositorioAluno = new AlunoServico(contexto);
        _repositorioTransacao = new TransacaoServico(contexto);
        _repositorioProfessor = new ProfessorServico(contexto);
    }
    public async Task TransferirMoedas(Transferencia transferencia)
    {
        await using var transacaoEntityFrameworkCore = await _contexto.Database.BeginTransactionAsync();
        
        try
        {
            var aluno = await _repositorioAluno.ObterPorCredencial(transferencia.IdentificadorAluno);
            var professor = await _repositorioProfessor.ObterPorCredencial(transferencia.IdentificadorProfessor);

            if (aluno is null || professor is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            if (professor.Moedas < transferencia.Moedas)
                throw new Exception(StatusCodes.Status400BadRequest.ToString());

            professor.Moedas -= transferencia.Moedas;
            aluno.Moedas += transferencia.Moedas;

            _contexto.Professores.Update(professor);
            _contexto.Alunos.Update(aluno);

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
            _email.EnviarEmail(new Email()
            {
                EmailDestino = aluno.Email,
                Assunto = "Moedas recebidas!",
                Mensagem = $"Você recebeu {transferencia.Moedas} moedas do professor {professor.Nome}"
            });

            await _contexto.SaveChangesAsync();
            await transacaoEntityFrameworkCore.CommitAsync();
        }
        catch
        {
            await transacaoEntityFrameworkCore.RollbackAsync();
        }
    }
}