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

public class EmpresaServico : IEmpresaRepositorio
{
    private readonly AppDbContexto _context;
    private readonly IConfiguration _configuration;
    public EmpresaServico(AppDbContexto context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<bool> Apagar(string credencial)
    {
        try
        {
            var empresaEncontrada = await _context.Empresas
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.CNPJ) ||
                    credencial.Equals(a.Email));
            if (empresaEncontrada is null) return false;

            _context.Empresas.Remove(empresaEncontrada);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Atualizar(string credencial, AtualizarEmpresaDto dto)
    {
        var success = false;
        try
        {
            var empresaEncontrada = await _context.Empresas
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.CNPJ) ||
                    credencial.Equals(a.Email));
            if (empresaEncontrada is null) return false;

            empresaEncontrada.Nome = dto.Nome;
            empresaEncontrada.Email = dto.Email;
            empresaEncontrada.Senha = dto.Senha;
            empresaEncontrada.CNPJ = dto.CNPJ;
            await _context.SaveChangesAsync();

            success = true;
            return success;
        }
        catch
        {
            return success;
        }
    }

    public async Task<Empresa> Criar(CadastrarEmpresaDto dto)
    {
        try
        {
            var aluno = new Empresa(dto);
            await _context.Empresas.AddAsync(aluno);
            await _context.SaveChangesAsync();
            return aluno;
        }
        catch
        {
            return null!;
        }
    }

    public async Task<MostrarEmpresaDto> ObterPorCredencial(string credencial)
    {
        try
        {
            var empresaEncontrada = await _context.Empresas
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.Email) ||
                    credencial.Equals(a.CNPJ)
                );
            if (empresaEncontrada == null) return null;
            return new MostrarEmpresaDto(empresaEncontrada);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<MostrarEmpresaDto>> ObterTodos()
    {
        try
        {
            var empresaFormat = new List<MostrarEmpresaDto>();
            var empresas = await _context.Empresas.ToListAsync();
            foreach (var empresa in empresas)
            {
                empresaFormat.Add(new MostrarEmpresaDto(empresa));
            }
            return empresaFormat;
        }
        catch
        {
            return new List<MostrarEmpresaDto>();
        }
    }

    public string GenerateNewJsonWebToken(List<Claim> claims)
    {
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
