using APIs.Contexto;
using APIs.Modelos.Dtos;
using APIs.Modelos.Entidade;
using APIs.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace APIs.Servicos;

public class VantagemServico : IVantagemRepositorio
{
    private readonly AppDbContexto _contexto;
    public VantagemServico(AppDbContexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<ICollection<Vantagem>> ObterTodos(int quantidadeDeItens)
    {
        var vantagens = await _contexto.Vantagens.ToListAsync();
        return vantagens.Count >= quantidadeDeItens ? vantagens.GetRange(0, quantidadeDeItens) : vantagens;
    }

    public async Task<Vantagem> ObterPorNome(string nome)
    {
        return await _contexto.Vantagens
            .FirstOrDefaultAsync(u => u.Nome.Equals(nome));
    }

    public async Task<Vantagem> Adicionar(CadastrarVantagemDto dto)
    {
        var vantagemId = Guid.NewGuid().ToString();
        Vantagem vantagem = new()
        {
            Id = vantagemId,
            Nome = dto.Nome,
            Valor = dto.Valor,
            Descricao = dto.Descricao,
            Foto = dto.Foto
        };

        await _contexto.Vantagens.AddAsync(vantagem);
        var resposta = await _contexto.SaveChangesAsync();
        return resposta != 0 ? vantagem : null!;
    }
    
    public async Task<Vantagem> Atualizar(string nome, AtualizarVantagemDto dto)
    {
        var vantagemEncontrada = await ObterPorNome(nome);
        if (vantagemEncontrada is null) return null;

        vantagemEncontrada.Nome = dto.Nome;
        vantagemEncontrada.Descricao = dto.Descricao;
        vantagemEncontrada.Valor = dto.Valor;
        vantagemEncontrada.Foto = dto.Foto;
        
        var resposta = await _contexto.SaveChangesAsync();
        return resposta != 0 ? vantagemEncontrada : null!;
    }

    public async Task<int> Apagar(string nome)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();
        try
        {
            
            var vantagem = await ObterPorNome(nome);
            if (vantagem is null) return 0;

            _contexto.Vantagens.Remove(vantagem);

            var vantagensAlunos = _contexto.VantagensAlunos.ToList();
            var todasVantagens = vantagensAlunos
                .FindAll(v => v.NomeDaVantagem.Equals(nome));
            
            _contexto.VantagensAlunos.RemoveRange(todasVantagens);
            var resposta = await _contexto.SaveChangesAsync();
            await transacao.CommitAsync();
            return resposta;
        }
        catch
        {
            await transacao.RollbackAsync();
            return 0;
        }
    }

    public async Task<Vantagem> ObterVantagemDeUmAluno(string identificadorAluno, string nomeVantagem="")
    {
        try
        {
            var vantagemAluno = await _contexto.VantagensAlunos
                .FirstOrDefaultAsync(v => v.IdentificadorAluno.Equals(identificadorAluno) &&
                                          v.NomeDaVantagem.Equals(nomeVantagem));

            if (vantagemAluno is null) return null!;
            return await _contexto.Vantagens.FirstOrDefaultAsync(v => v.Nome.Equals(vantagemAluno.NomeDaVantagem));;
        }
        catch
        {
            return null!;
        }
    }

    public async Task<Vantagem> TrocarMoedas(string identificadorAluno, string nomeVantagem)
    {
        await using var transacao = await _contexto.Database.BeginTransactionAsync();
        try
        {
            var aluno = 
                await _contexto.Usuarios.FirstOrDefaultAsync(a => a.Identificador.Equals(identificadorAluno));
            if (aluno is null) return null!;

            var contaBancaria =
                await _contexto.Contas.FirstOrDefaultAsync(b => b.Identificador.Equals(identificadorAluno));
            if (contaBancaria is null) return null!;
            if (contaBancaria.SaldoBancario == 0) return null!;

            var vantagem = await _contexto.Vantagens.FirstOrDefaultAsync(v => v.Nome.Equals(nomeVantagem));
            if (vantagem is null) return null!;
            if (contaBancaria.SaldoBancario < vantagem.Valor) return null!;

            contaBancaria.SaldoBancario -= vantagem.Valor;
            _contexto.Contas.Update(contaBancaria);
            await _contexto.VantagensAlunos.AddAsync(new VantagemDeCadaAluno()
            {
                Id = Guid.NewGuid().ToString(),
                IdentificadorAluno = identificadorAluno,
                NomeDaVantagem = vantagem.Nome,
                ObtidaEm = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                ValorDaVantagem = vantagem.Valor
            });
            
            var resposta = await _contexto.SaveChangesAsync();

            if (resposta == 0) await transacao.RollbackAsync();
            await transacao.CommitAsync();
            return vantagem;
        }
        catch
        {
            await transacao.RollbackAsync();
            return null!;
        }
    }
}