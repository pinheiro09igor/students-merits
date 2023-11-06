using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IEmail
{
    Task EnviarEmail(Email email);
}