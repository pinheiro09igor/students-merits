using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;

namespace api.merito.estudantil.servicos;

public class ResgateVantagemServico : IRepositorioResgateVantagem
{
    private readonly Contexto _contexto;
    private readonly IEmail _email;
    private readonly IRepositorioGenerico<Aluno> _repositorioAluno;
    private readonly IRepositorioVantagem _repositorioVantagem;
    private readonly IRepositorioTransacao _repositorioTransacao;
    private readonly IRepositorioGenerico<Empresa> _repositorioEmpresa;
    
    public ResgateVantagemServico(Contexto contexto)
    {
        _contexto = contexto;
        _email = new EmailServico();
        _repositorioAluno = new AlunoServico(contexto);
        _repositorioVantagem = new VantagemServico(contexto);
        _repositorioTransacao = new TransacaoServico(contexto);
        _repositorioEmpresa = new EmpresaServico(contexto);
    }
    
    public async Task ResgatarVantagem(ResgatarVantagem resgatarVantagem)
    {
        await using var transacaoEntityFrameworkCore = await _contexto.Database.BeginTransactionAsync();

        try
        {
            var aluno = await _repositorioAluno.ObterPorCredencial(resgatarVantagem.IdentificadorAluno);
            if (aluno is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            var vantagem = await _repositorioVantagem.ObterPorCredencial(resgatarVantagem.IdentificadorVantagem);
            if(vantagem is null) 
                throw new Exception(StatusCodes.Status404NotFound.ToString());

            if ((aluno.Moedas - vantagem.Valor) < 0) 
                throw new Exception(StatusCodes.Status400BadRequest.ToString());

            vantagem.ResgatadaPor = aluno.Id;
            aluno.Moedas -= vantagem.Valor;

            _contexto.Alunos.Update(aluno);
            _contexto.Vantagens.Update(vantagem);

            await _repositorioTransacao.CriarTransacao(new Transacao()
            {
                Id = Guid.NewGuid().ToString(),
                RemetenteIdentificador = aluno.Id,
                DestinatarioIdentificador = vantagem.IdEmpresa,
                Valor = vantagem.Valor,
                VantagemIdentificador = vantagem.Id,
                Data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                Descricao = $"Vantagem resgatada de nome: '{vantagem.Nome}'"
            });
            _email.EnviarEmail(new Email()
            {
                EmailDestino = aluno.Email,
                Assunto = "Você resgatou uma vantagem!",
                Mensagem =
                    $"Você resgatou uma vantagem de nome '{vantagem.Nome}' por {vantagem.Valor} moedas. Código para resgate: {vantagem.Id}"
            });
            
            var empresa = await _repositorioEmpresa.ObterPorCredencial(vantagem.IdEmpresa);
            if (empresa is not null)
                _email.EnviarEmail(new Email()
                {
                    EmailDestino = empresa.Email,
                    Assunto = "Uma vantagem foi resgatada!",
                    Mensagem = $"A vantagem de nome '{vantagem.Nome} foi resgatada pelo aluno '{aluno.Nome}' com sucesso!"
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