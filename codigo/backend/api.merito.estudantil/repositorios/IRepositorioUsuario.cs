using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IRepositorioUsuario
{
    Task Login(Login login);
    Task<Saldo> ObterSaldo(string credencial);
}