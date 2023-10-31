using APIs.Contexto;
using APIs.Modelos.Dtos;
using APIs.Modelos.Entidade;
using APIs.Modelos.Enum;
using APIs.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace APIs.Servicos;

public class UsuarioServico : IUsuarioRepositorio
{
    private readonly AppDbContexto _contexto;
    public UsuarioServico(AppDbContexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<ICollection<Usuario>> ObterTodos(int quantidadeDeItens)
    {

        var usuarios = await _contexto.Usuarios.Include(e => e.EnderecoDoUsuario).ToListAsync();
        return usuarios.Count >= quantidadeDeItens ? usuarios.GetRange(0, quantidadeDeItens) : usuarios;
    }

    public async Task<Usuario> ObterPorIdentificador(string identificador)
    {
        return await _contexto.Usuarios
            .Include(e => e.EnderecoDoUsuario)
            .FirstOrDefaultAsync(u => u.Identificador.Equals(identificador));
    }

    public async Task<Usuario> Adicionar(CadastrarUsuarioDto dto)
    {
        if (!EnumTipoDeUsuario.ALUNO.ToString().Equals(dto.TipoDeUsuario) &&
            !EnumTipoDeUsuario.EMPRESA.ToString().Equals(dto.TipoDeUsuario) &&
            !EnumTipoDeUsuario.PROFESSOR.ToString().Equals(dto.TipoDeUsuario)) return null!;

        await using var transacao = await _contexto.Database.BeginTransactionAsync();
        try
        {
            var usuarioId = Guid.NewGuid().ToString();
            var usuario = new Usuario
            {
                Id = usuarioId,
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                Identificador = dto.Identificador,
                TipoDeUsuario = dto.TipoDeUsuario,
                EnderecoDoUsuario = new Endereco
                {
                    Id = Guid.NewGuid().ToString(),
                    Rua = dto.EnderecoDoUsuario.Rua,
                    Numero = dto.EnderecoDoUsuario.Numero,
                    Bairro = dto.EnderecoDoUsuario.Bairro,
                    Cidade = dto.EnderecoDoUsuario.Cidade,
                    Estado = dto.EnderecoDoUsuario.Estado,
                    Cep = dto.EnderecoDoUsuario.Cep,
                    UsuarioId = usuarioId,
                },
            };
            usuario.EnderecoDoUsuario.UsuarioQuePertenceAEsseEndereco = usuario;

            var conta = new ContaBancaria
            {
                Id = Guid.NewGuid().ToString(),
                Identificador = dto.Identificador,
                SaldoBancario = 0
            };

            if (usuario.TipoDeUsuario.Equals("PROFESSOR"))
                conta.SaldoBancario = 1000.0;

            await _contexto.Usuarios.AddAsync(usuario);
            await _contexto.Contas.AddAsync(conta);
            var resposta = await _contexto.SaveChangesAsync();
            
            await transacao.CommitAsync();
            return resposta != 0 ? usuario : null!;
        }
        catch
        {
            await transacao.RollbackAsync();
            return null!;
        }
    }

    public async Task<Usuario> Atualizar(string identificador, AtualizarUsuarioDto dto)
    {
        if (!EnumTipoDeUsuario.ALUNO.ToString().Equals(dto.TipoDeUsuario) &&
            !EnumTipoDeUsuario.EMPRESA.ToString().Equals(dto.TipoDeUsuario) &&
            !EnumTipoDeUsuario.PROFESSOR.ToString().Equals(dto.TipoDeUsuario)) return null!;

        try
        {
            var usuarioEncontrado = await ObterPorIdentificador(identificador);
            if (usuarioEncontrado is null) return null;

            usuarioEncontrado.Nome = dto.Nome;
            usuarioEncontrado.Email = dto.Email;
            usuarioEncontrado.Senha = dto.Senha;
            usuarioEncontrado.Identificador = dto.Identificador;
            usuarioEncontrado.TipoDeUsuario = dto.TipoDeUsuario;
            usuarioEncontrado.EnderecoDoUsuario.Rua = dto.EnderecoDoUsuario.Rua;
            usuarioEncontrado.EnderecoDoUsuario.Numero = dto.EnderecoDoUsuario.Numero;
            usuarioEncontrado.EnderecoDoUsuario.Bairro = dto.EnderecoDoUsuario.Bairro;
            usuarioEncontrado.EnderecoDoUsuario.Bairro = dto.EnderecoDoUsuario.Bairro;
            usuarioEncontrado.EnderecoDoUsuario.Cidade = dto.EnderecoDoUsuario.Cidade;
            usuarioEncontrado.EnderecoDoUsuario.Estado = dto.EnderecoDoUsuario.Estado;
            usuarioEncontrado.EnderecoDoUsuario.Cep = dto.EnderecoDoUsuario.Cep;
            
            var resposta = await _contexto.SaveChangesAsync();
            return resposta != 0 ? usuarioEncontrado : null!;
        }
        catch
        {
            return null!;
        }
    }

    public async Task<int> Apagar(string identificador)
    {
        try
        {
            var usuario = await ObterPorIdentificador(identificador);
            if (usuario is null) return 0;

            _contexto.Usuarios.Remove(usuario);
            return await _contexto.SaveChangesAsync();
        }
        catch
        {
            return 0;
        }
    }
}