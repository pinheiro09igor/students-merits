namespace APIs.Modelos.Dtos;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Tipo {  get; set; } = string.Empty;
}
