using APIs.Modelos.Entidade;

namespace APIs.Modelos.Dtos;

public class MostrarEndereco
{
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Cep { get; set; }

    public MostrarEndereco(Endereco endereco)
    {
        Rua = endereco.Rua;
        Numero = endereco.Numero;
        Bairro = endereco.Bairro;
        Cidade = endereco.Cidade;
        Estado = endereco.Estado;
        Cep = endereco.Cep;
    }
}