using APIs.Contexto;
using APIs.Interfaces;
using APIs.Modelos;
using APIs.Modelos.Dtos;
using Microsoft.EntityFrameworkCore;

namespace APIs.Servicos;

public class AlunoServico : IAlunoRepositorio
{
    private readonly AppDbContexto _context;
    public AlunoServico(AppDbContexto context)
    {
        _context = context;
    }

    public async Task<bool> Criar(CadastrarAlunoDto dto)
    {
        var success = false;
        try
        {
            var aluno = new Aluno(dto);
            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();
            success = true;
            return success;
        } catch
        {
            return success;
        }
    }

    public async Task<bool> Atualizar(string credencial, AtualizarAlunoDto dto)
    {
        var success = false;
        try
        {
            var alunoEncontrado = await _context.Alunos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.Email) || 
                    credencial.Equals(a.CPF) || 
                    credencial.Equals(a.RG)
                );
            if (alunoEncontrado is null) return false;

            alunoEncontrado.Nome = dto.Nome;
            alunoEncontrado.Email = dto.Email;
            alunoEncontrado.Senha = dto.Senha;
            alunoEncontrado.RG = dto.RG;
            alunoEncontrado.CPF = dto.CPF;
            alunoEncontrado.Endereco.Rua = dto.Endereco.Rua;
            alunoEncontrado.Endereco.Numero = dto.Endereco.Numero;
            alunoEncontrado.Endereco.Bairro = dto.Endereco.Bairro;
            alunoEncontrado.Endereco.Cidade = dto.Endereco.Cidade;
            alunoEncontrado.Endereco.CEP = dto.Endereco.CEP;
            alunoEncontrado.InstituicaoDeEnsino = dto.InstituicaoDeEnsino;
            await _context.SaveChangesAsync();

            success = true;
            return success;
        }
        catch
        {
            return success;
        }
    }

    public async Task<List<MostrarAlunoDto>> ObterTodos()
    {
        try
        {
            var alunosFormat = new List<MostrarAlunoDto>();
            var alunos = await _context.Alunos.Include(e => e.Endereco).ToListAsync();
            foreach (var aluno in alunos)
            {
                alunosFormat.Add(new MostrarAlunoDto(aluno));
            }
            return alunosFormat;
        } catch
        {
            return new List<MostrarAlunoDto>();
        }
    }

    public async Task<MostrarAlunoDto> ObterPorCredencial(string credencial)
    {
        try
        {
            var alunosFormat = new List<MostrarAlunoDto>();
            var alunoEncontrado = await _context.Alunos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.Email) ||
                    credencial.Equals(a.CPF) ||
                    credencial.Equals(a.RG)
                );
            return new MostrarAlunoDto(alunoEncontrado);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> Apagar(string credencial)
    {
        try
        {
            var alunoEncontrado = await _context.Alunos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.Email) ||
                    credencial.Equals(a.CPF) ||
                    credencial.Equals(a.RG)
                );
            if (alunoEncontrado is null) return false;

            _context.Alunos.Remove(alunoEncontrado);
            _context.Enderecos.Remove(alunoEncontrado.Endereco);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
