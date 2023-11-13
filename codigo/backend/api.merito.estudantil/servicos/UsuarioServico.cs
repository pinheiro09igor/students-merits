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
    
    public async Task<Usuario> Login(Login login)
    {
        var usuario = 
            await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(login.Email) && u.Senha.Equals(login.Senha)) ?? throw new Exception(StatusCodes.Status404NotFound.ToString());

        return usuario;
    }

    public async Task<Saldo> ObterSaldo(string credencial)
    {
        var usuario =
            await _contexto.Usuarios.FirstOrDefaultAsync(a => a.Id.Equals(credencial)) 
            ?? throw new Exception(StatusCodes.Status404NotFound.ToString());

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

        

        if(usuario.Tipo.Equals("aluno"))
        {
            var usuarioEncontrado = await _contexto.Alunos.FirstOrDefaultAsync(u => u.Id.Equals(usuario.Id));
            return new Saldo()
            {
                Moedas = usuarioEncontrado.Moedas,
                Transacoes = transacoesComNome
            };
        } 
        else
        {
            var usuarioEncontrado = await _contexto.Professores.FirstOrDefaultAsync(u => u.Id.Equals(usuario.Id));
            return new Saldo()
            {
                Moedas = usuarioEncontrado.Moedas,
                Transacoes = transacoesComNome
            };
        }
    }
}