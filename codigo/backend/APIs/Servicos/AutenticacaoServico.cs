using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIs.Modelos.Dtos;
using APIs.Repositorios;
using Microsoft.IdentityModel.Tokens;

namespace APIs.Servicos;

public class AutenticacaoServico : IAutenticacaoRepositorio
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly string _validIssuer;
    private readonly string _validAudience;
    private readonly SymmetricSecurityKey _chaveSecreta;
    public AutenticacaoServico(IUsuarioRepositorio repositorio, IConfiguration configuration)
    {
        _repositorio = repositorio;
        _validIssuer = configuration["Jwt:ValidIssuer"]!;
        _validAudience = configuration["Jwt:ValidAudience"]!;
        var secret = configuration["Jwt:Secret"]!;
        var key = Encoding.UTF8.GetBytes(secret);
        _chaveSecreta = new SymmetricSecurityKey(key);
    }
    
    public async Task<string> Logar(LoginDto dto)
    {
        var usuario = await _repositorio.ObterPorIdentificador(dto.CredencialDeAcesso);
        if (usuario == null) return null!;
        if (!usuario.Senha.Equals(dto.Senha)) return null!;

        return GerarToken(new List<Claim>()
        {
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.NameIdentifier, usuario.Identificador)
        });
    }

    public bool ValidarToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _validIssuer,
            ValidateAudience = true,
            ValidAudience = _validAudience,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _chaveSecreta
        };
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var result = tokenHandler.ValidateToken(token, validationParameters, out _);
            return result is not null;
        }
        catch
        {
            return false;
        }
    }

    private string GerarToken(IEnumerable<Claim> claims)
    {
        var credenciais = new SigningCredentials(_chaveSecreta, SecurityAlgorithms.HmacSha256);
        var objetoDeToken = new JwtSecurityToken(
            issuer: _validIssuer,
            audience: _validAudience,
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: credenciais);
        return new JwtSecurityTokenHandler().WriteToken(objetoDeToken);
    }
}