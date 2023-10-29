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
    private readonly IConfiguration _configuration;
    private string _validIssuer;
    private string _validAudience;
    private SymmetricSecurityKey _chaveSecreta;
    public AutenticacaoServico(IUsuarioRepositorio repositorio, IConfiguration configuration)
    {
        _repositorio = repositorio;
        _configuration = configuration;
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
        _validIssuer = _configuration["Jwt:ValidIssuer"]!;
        _validAudience = _configuration["Jwt:ValidAudience"]!;
        var secret = _configuration["Jwt:Secret"]!;
        var key = Encoding.UTF8.GetBytes(secret);
        _chaveSecreta = new SymmetricSecurityKey(key);
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
        _validIssuer = _configuration["Jwt:ValidIssuer"]!;
        _validAudience = _configuration["Jwt:ValidAudience"]!;
        var secret = _configuration["Jwt:Secret"]!;
        var key = Encoding.UTF8.GetBytes(secret);
        _chaveSecreta = new SymmetricSecurityKey(key);
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