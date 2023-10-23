namespace APIs.Modelos.Dtos;

public class MostrarAlunoDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string RG { get; set; }
    public string CPF { get; set; }
    public Endereco Endereco { get; set; }
    public string InstituicaoDeEnsino { get; set; }

    public MostrarAlunoDto()
    {
        
    }

    public MostrarAlunoDto(Aluno a)
    {
        Nome = a.Nome;
        Email = a.Email;
        RG = a.RG;
        CPF = a.CPF;
        Endereco = a.Endereco;
        InstituicaoDeEnsino = a.InstituicaoDeEnsino;
    }
}
