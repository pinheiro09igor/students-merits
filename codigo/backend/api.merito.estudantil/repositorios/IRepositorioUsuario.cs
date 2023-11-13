using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IRepositorioUsuario
{
    Task<Usuario> Login(Login login);
    Task<Saldo> ObterSaldo(string credencial);
}