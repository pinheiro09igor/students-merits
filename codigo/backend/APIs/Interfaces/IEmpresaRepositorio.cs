using APIs.Modelos;
using APIs.Modelos.Dtos;

namespace APIs.Interfaces;

public interface IEmpresaRepositorio
{
    /// <summary>
    /// Obter todas as empresas do banco de dados.
    /// </summary>
    /// <returns>Lista de empresas obtidas do banco de dados.</returns>
    Task<List<MostrarEmpresaDto>> ObterTodos();

    /// <summary>
    /// Obter uma empresa do banco de dados de acordo com uma credencial referente.
    /// </summary>
    /// <param name="credencial">Credencial que representa um dado da empresa do banco de dados.</param>
    /// <returns>Empresa obtida do banco de dados.</returns>
    Task<MostrarEmpresaDto> ObterPorCredencial(string credencial);

    /// <summary>
    /// Criar uma empresa no banco de dados.
    /// </summary>
    /// <param name="dto">Dados enviados no corpo da requisição.</param>
    /// <returns>Retorna verdadeiro caso a empresa seja cadastrada com sucesso.</returns>
    Task<bool> Criar(CadastrarEmpresaDto dto);

    /// <summary>
    /// Atualizar uma empresa no banco de dados.
    /// </summary>
    /// <param name="credencial">Credencial que representa um dado da empresa no banco de dados.</param>
    /// <param name="dto">Dados enviados no corpo da requisição.</param>
    /// <returns>Retorna verdadeiro caso a empresa seja atualizada com sucesso.</returns>
    Task<bool> Atualizar(string credencial, AtualizarEmpresaDto dto);

    /// <summary>
    /// Apagar uma empresa do banco de dados.
    /// </summary>
    /// <param name="credencial">Credencial que representa um dado da empresa do banco de dados.</param>
    /// <returns>Retorna verdadeiro caso a empresa seja apagada com sucesso.</returns>
    Task<bool> Apagar(string credencial);

    /// <summary>
    /// Obter a entidade Empresa do banco de dados de acordo com uma credencial.
    /// </summary>
    /// <param name="credencial">Credencial que representa um dado de uma empresa do banco de dados.</param>
    /// <returns>Retorna uma entidade empresa do banco de dados.</returns>
    Task<Empresa> ObterEmpresaPorCredencial(string credencial);
}
