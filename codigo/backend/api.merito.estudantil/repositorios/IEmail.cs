using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IEmail
{
    void EnviarEmail(Email email);
}