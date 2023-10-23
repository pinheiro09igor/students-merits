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

    public async Task<MostrarEmpresaDto> ObterPorCredencial(string credencial)
    {
        try
        {
            var empresaEncontrada = await ObterEmpresaPorCredencial(credencial);
            if (empresaEncontrada == null) return null;
            return new MostrarEmpresaDto(empresaEncontrada);
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
            var empresaEncontrada = await ObterEmpresaPorCredencial(credencial);
            if (empresaEncontrada is null) return false;

            _context.Empresas.Remove(empresaEncontrada);
            return (await _context.SaveChangesAsync() != 0);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Atualizar(string credencial, AtualizarEmpresaDto dto)
    {
        try
        {
            var empresaEncontrada = await ObterEmpresaPorCredencial(credencial);
            if (empresaEncontrada is null) return false;

            empresaEncontrada.Nome = dto.Nome;
            empresaEncontrada.Email = dto.Email;
            empresaEncontrada.Senha = dto.Senha;
            empresaEncontrada.CNPJ = dto.CNPJ;

            return (await _context.SaveChangesAsync() != 0);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Criar(CadastrarEmpresaDto dto)
    {
        try
        {
            Empresa empresa = new(dto);
            await _context.Empresas.AddAsync(empresa);
            return (await _context.SaveChangesAsync() != 0);
        }
        catch
        {
            return false;
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

    public async Task<Empresa> ObterEmpresaPorCredencial(string credencial)
    {
        try
        {
            var empresaEncontrada = await _context.Empresas
                .FirstOrDefaultAsync(
                    a => credencial.Equals(a.Email) ||
                    credencial.Equals(a.CNPJ)
                );
            if (empresaEncontrada == null) return null;
            return empresaEncontrada;
        }
        catch
        {
            return null;
        }
    }
}
