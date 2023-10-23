using APIs.Modelos;
using APIs.Modelos.Dtos;

namespace APIs.Interfaces;

public interface IAlunoRepositorio
{
    /// <summary>
    /// Obtém todos os alunos do banco de dados.
    /// </summary>
    /// <returns>Retorna uma lista de alunos que mostra somente as informações necessárias.</returns>
    Task<List<MostrarAlunoDto>> ObterTodos();

    /// <summary>
    /// Obtém um usuário do banco de dados por uma credencial.
    /// </summary>
    /// <param name="credencial">Credencial que representa um dado do usuário que está no banco de dados.</param>
    /// <returns>Retorna os dados de um usuário do banco de dados.</returns>
    Task<MostrarAlunoDto> ObterPorCredencial(string credencial);

    /// <summary>
    /// Cadastrar um usuário no banco de dados.
    /// </summary>
    /// <param name="dto">Dados enviados no corpo da requisição.</param>
    /// <returns>Retorna verdadeiro caso o cadastro seja efetuado com sucesso, falso caso dê algum erro.</returns>
    Task<bool> Criar(CadastrarAlunoDto dto);

    /// <summary>
    /// Atualizar um usuário no banco de dados.
    /// </summary>
    /// <param name="credencial">Credencial na qual um usuário existente será obtido do banco de dados.</param>
    /// <param name="dto">Dados enviados no corpo da requisição.</param>
    /// <returns>Retorna verdadeiro caso a atualização seja efetuada com sucesso, falso caso dê algum erro.</returns>
    Task<bool> Atualizar(string credencial, AtualizarAlunoDto dto);

    /// <summary>
    /// Apagar um usuário do banco de dados.
    /// </summary>
    /// <param name="credencial">Credencial na qual um usuário existente será obtido do banco de dados.</param>
    /// <returns>Retorna verdadeiro caso a atualização seja efetuada com sucesso, falso caso dê algum erro.</returns>
    Task<bool> Apagar(string credencial);

    /// <summary>
    /// Obtém um usuário do banco de dados por uma credencial.
    /// </summary>
    /// <param name="credencial">Credencial que representa um dado do usuário que está no banco de dados.</param>
    /// <returns>Retorna os dados de um usuário do banco de dados.</returns>
    Task<Aluno> ObterAlunoPorCredencial(string credencial);
}
