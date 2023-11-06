using api.merito.estudantil.contexto;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;
using Microsoft.EntityFrameworkCore;

namespace api.merito.estudantil.servicos;

public class UsuarioServico : IRepositorioUsuario
{
    private readonly Contexto _contexto;
    public UsuarioServico(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task Login(Login login)
    {
        var usuario = 
            await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(login.Email) && u.Senha.Equals(login.Senha));
        if (usuario is null) 
            throw new Exception(StatusCodes.Status404NotFound.ToString());
    }

    public async Task<Saldo> ObterSaldo(string credencial)
    {
        var usuario =
            await _contexto.Alunos.FirstOrDefaultAsync(a => a.Id.Equals(credencial) || a.Email.Equals(credencial) || a.Rg.Equals(credencial));
        
        if (usuario is null) 
            throw new Exception(StatusCodes.Status404NotFound.ToString());

        var transacoesComNome = new List<TransacaoComNome>();
        var transacoesDoRemetenteEDoDestinatario = new List<Transacao>();
        var transacoes = await _contexto.Transacoes.ToListAsync();

        var transacoesObtidas = transacoes.FindAll(
            t => t.RemetenteIdentificador.Equals(credencial) || t.DestinatarioIdentificador.Equals(credencial));
        transacoesDoRemetenteEDoDestinatario.AddRange(transacoesObtidas);

        foreach (var transacao in transacoesDoRemetenteEDoDestinatario)
        {
            var remetenteIdent = transacao.RemetenteIdentificador;
            var remetente = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(remetenteIdent) 
                                                                              || u.Email.Equals(remetenteIdent));

            var destinatarioIdent = transacao.DestinatarioIdentificador;
            var destinatario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(destinatarioIdent) || u.Email.Equals(destinatarioIdent));
            
            transacoesComNome.Add(new TransacaoComNome
            {
                Id = Guid.NewGuid().ToString(),
                DestinatarioIdentificador = destinatarioIdent,
                DestinatarioNome = destinatario.Nome,
                RemetenteIdentificador = remetenteIdent,
                RemetenteNome = remetente.Nome,
                Valor = transacao.Valor,
                Data = transacao.Data,
                Descricao = transacao.Descricao,
                VantagemIdentificador = transacao.VantagemIdentificador
            });
        }

        return new Saldo()
        {
            Moedas = usuario.Moedas,
            Transacoes = transacoesComNome
        };
    }
}