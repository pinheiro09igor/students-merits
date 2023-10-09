using APIs.Modelos.Dtos;

namespace APIs.Modelos;

public class Empresa : Usuario
{
    public string CNPJ { get; set; }

    public Empresa()
    {

    }

    public Empresa(CadastrarEmpresaDto dto)
    {
        Id = Guid.NewGuid().ToString();
        Nome = dto.Nome;
        Email = dto.Email;
        Senha = dto.Senha;
        CNPJ = dto.CNPJ;
    }
}
