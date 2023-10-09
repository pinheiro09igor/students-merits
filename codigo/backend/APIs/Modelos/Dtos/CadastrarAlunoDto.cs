namespace APIs.Modelos.Dtos;

public class CadastrarAlunoDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string RG { get; set; }
    public string CPF { get; set; }
    public Endereco? Endereco { get; set; }
    public string InstituicaoDeEnsino { get; set; }
}
