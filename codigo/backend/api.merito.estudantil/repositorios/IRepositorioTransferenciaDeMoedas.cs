using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IRepositorioTransferenciaDeMoedas
{
    Task TransferirMoedas(Transferencia transferencia);
}