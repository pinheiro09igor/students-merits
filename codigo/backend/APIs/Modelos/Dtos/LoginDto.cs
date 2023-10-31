using System.Text.Json.Serialization;

namespace APIs.Modelos.Dtos;

public class LoginDto
{
    public string CredencialDeAcesso { get; set; }
    public string Senha { get; set; }
}