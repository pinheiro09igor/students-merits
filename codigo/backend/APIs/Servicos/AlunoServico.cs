using APIs.Contexto;
using APIs.Interfaces;
using APIs.Modelos;
using APIs.Modelos.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIs.Servicos;

public class AlunoServico : IAlunoRepositorio
{
    private readonly AppDbContexto _context;
    private readonly IConfiguration _configuration;
    public AlunoServico(AppDbContexto context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<List<MostrarAlunoDto>> ObterTodos()
    {
        try
        {
            var alunosResponse = new List<MostrarAlunoDto>();
            var alunos = await _context.Alunos.Include(e => e.Endereco).ToListAsync();

            foreach (var aluno in alunos)
            {
                alunosResponse.Add(new MostrarAlunoDto(aluno));
            }
            return alunosResponse;
        }
        catch
        {
            return new List<MostrarAlunoDto>();
        }
    }

    public async Task<MostrarAlunoDto> ObterPorCredencial(string credencial)
    {
        try
        {
            var alunoEncontrado = await ObterAlunoPorCredencial(credencial);
            if (alunoEncontrado is null) return null;
            return new MostrarAlunoDto(alunoEncontrado);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> Criar(CadastrarAlunoDto dto)
    {
        try
        {
            Aluno aluno = new(dto);
            await _context.Alunos.AddAsync(aluno);
            return (await _context.SaveChangesAsync() != 0);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Atualizar(string credencial, AtualizarAlunoDto dto)
    {
        try
        {
            var alunoEncontrado = await ObterAlunoPorCredencial(credencial);
            if (alunoEncontrado is null) return false;

            GerarAlunoAtualizado(alunoEncontrado, dto);

            return (await _context.SaveChangesAsync() != 0);
        }
        catch
        {
            return false;
        }
    }


    public async Task<bool> Apagar(string credencial)
    {
        try
        {
            var alunoEncontrado = await ObterAlunoPorCredencial(credencial);
            if (alunoEncontrado is null) return false;

            _context.Alunos.Remove(alunoEncontrado);
            _context.Enderecos.Remove(alunoEncontrado.Endereco);
            
            return (await _context.SaveChangesAsync() != 0);
        }
        catch
        {
            return false;
        }
    }

    public async Task<Aluno> ObterAlunoPorCredencial(string credencial)
    {
        try
        {
            var alunoEncontrado = await _context.Alunos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(a => credencial.Equals(a.Email) || credencial.Equals(a.CPF) || credencial.Equals(a.RG));

            if (alunoEncontrado is null) return null;
            return alunoEncontrado;
        }
        catch
        {
            return null;
        }
    }

    private Aluno GerarAlunoAtualizado(Aluno alunoEncontrado, AtualizarAlunoDto dto)
    {
        alunoEncontrado.Nome = dto.Nome;
        alunoEncontrado.Email = dto.Email;
        alunoEncontrado.RG = dto.RG;
        alunoEncontrado.CPF = dto.CPF;
        alunoEncontrado.Senha = dto.Senha;
        alunoEncontrado.Endereco.Aluno = alunoEncontrado;
        alunoEncontrado.Endereco.Rua = dto.Endereco.Rua;
        alunoEncontrado.Endereco.Numero = dto.Endereco.Numero;
        alunoEncontrado.Endereco.Bairro = dto.Endereco.Bairro;
        alunoEncontrado.Endereco.Cidade = dto.Endereco.Cidade;
        alunoEncontrado.Endereco.CEP = dto.Endereco.CEP;
        alunoEncontrado.InstituicaoDeEnsino = dto.InstituicaoDeEnsino;
        return alunoEncontrado;
    }
}

