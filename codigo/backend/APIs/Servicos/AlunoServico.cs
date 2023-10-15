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

    public async Task<Aluno> Criar(CadastrarAlunoDto dto)
    {
        try
        {
            var aluno = new Aluno(dto);
            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();
            return aluno;
        }
        catch
        {
            return null!;
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
            var alunosFormat = new List<MostrarAlunoDto>();
            var alunoEncontrado = await _context.Alunos
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.Email) ||
                    credencial.Equals(a.CPF) ||
                    credencial.Equals(a.RG)
                );
            if (alunoEncontrado == null) return null;
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

    public string GenerateNewJsonWebToken(List<Claim> claims)
    {
        var validIssuer = _configuration["Jwt:ValidIssuer"]!;
        var validAudience = _configuration["Jwt:ValidAudience"]!;
        var secret = _configuration["Jwt:Secret"]!;
        var key = Encoding.UTF8.GetBytes(secret);

        var chaveSecreta = new SymmetricSecurityKey(key);
        var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

        var objetoDeToken = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: credenciais);
        string token = new JwtSecurityTokenHandler().WriteToken(objetoDeToken);
        return token;
    }
}

