namespace APIs.Modelos.Dtos;

public class MostrarEmpresaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;

    public MostrarEmpresaDto()
    {
        
    }

    public MostrarEmpresaDto(Empresa e)
    {
        Nome = e.Nome;
        Email = e.Email;
        Senha = e.Senha;
        CNPJ = e.CNPJ;
    }
}
