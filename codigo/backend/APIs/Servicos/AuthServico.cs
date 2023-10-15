using APIs.Interfaces;
using APIs.Modelos;
using APIs.Modelos.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIs.Servicos
{
    public class AuthServico : IAuthRepositorio
    {
        private readonly IAlunoRepositorio alunoRepositorio;
        private readonly IEmpresaRepositorio empresaRepositorio;
        private readonly IConfiguration config;

        public AuthServico(IAlunoRepositorio alunoRepositorio, IEmpresaRepositorio empresaRepositorio, IConfiguration config)
        {
            this.alunoRepositorio = alunoRepositorio;
            this.empresaRepositorio = empresaRepositorio;
            this.config = config;
        }

        public async Task<string> Cadastrar(CadastrarEmpresaDto dto)
        {
            var empresa = await empresaRepositorio.Criar(dto);
            if (empresa != null) return GenerateNewJsonWebToken(new List<Claim>()
            {
                new Claim(ClaimTypes.Email, empresa.Email),
                new Claim(ClaimTypes.Name, empresa.Nome),
                new Claim(ClaimTypes.NameIdentifier, empresa.CNPJ)
            });
            return null!;
        }

        public async Task<string> Cadastrar(CadastrarAlunoDto dto)
        {
            var aluno = await alunoRepositorio.Criar(dto);
            if (aluno != null) return GenerateNewJsonWebToken(new List<Claim>()
            {
                new Claim(ClaimTypes.Email, aluno.Email),
                new Claim(ClaimTypes.Name, aluno.Nome),
                new Claim(ClaimTypes.NameIdentifier, aluno.CPF)
            });
            return null!;
        }

        public async Task<string> Logar(LoginDto dto)
        {
            if (dto.Tipo.Equals("ALUNO"))
            {
                var aluno = await alunoRepositorio.ObterPorCredencial(dto.Email);
                if (aluno == null) return null!;
                if (!aluno.Senha.Equals(dto.Senha)) return null!;

                return GenerateNewJsonWebToken(new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, aluno.Email),
                    new Claim(ClaimTypes.Name, aluno.Nome),
                    new Claim(ClaimTypes.NameIdentifier, aluno.CPF)
                });
            }
            else if (dto.Tipo.Equals("EMPRESA"))
            {
                var empresa = await empresaRepositorio.ObterPorCredencial(dto.Email);
                if (empresa == null) return null!;
                if (!empresa.Senha.Equals(dto.Senha)) return null!;

                return GenerateNewJsonWebToken(new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, empresa.Email),
                    new Claim(ClaimTypes.Name, empresa.Nome),
                    new Claim(ClaimTypes.NameIdentifier, empresa.CNPJ)
                });
            } 
            else
            {
                return null!;
            }
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var validIssuer = config["Jwt:ValidIssuer"]!;
            var validAudience = config["Jwt:ValidAudience"]!;
            var secret = config["Jwt:Secret"]!;
            var key = Encoding.UTF8.GetBytes(secret);
            var chaveSecreta = new SymmetricSecurityKey(key);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = validIssuer,
                ValidateAudience = true,
                ValidAudience = validAudience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = chaveSecreta
            };
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var result = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return result;
            }
            catch
            {
                return null!;
            }
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var validIssuer = config["Jwt:ValidIssuer"]!;
            var validAudience = config["Jwt:ValidAudience"]!;
            var secret = config["Jwt:Secret"]!;
            var key = Encoding.UTF8.GetBytes(secret);

            var chaveSecreta = new SymmetricSecurityKey(key);
            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            var objetoDeToken = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: credenciais);
            string token = new JwtSecurityTokenHandler().WriteToken(objetoDeToken);
            return token;
        }
    }
}
