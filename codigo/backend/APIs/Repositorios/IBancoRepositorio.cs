using APIs.Modelos.Dtos;
using APIs.Modelos.Entidade;

namespace APIs.Repositorios;

public interface IBancoRepositorio
{
    Task<bool> Transferir(RealizarTransferenciaDto dto);
    Task<ExtratoBancario> GerarExtrato(string identificador);
}
