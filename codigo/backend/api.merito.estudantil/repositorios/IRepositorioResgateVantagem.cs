using api.merito.estudantil.models;

namespace api.merito.estudantil.repositorios;

public interface IRepositorioResgateVantagem
{
    Task ResgatarVantagem(ResgatarVantagem resgatarVantagem);
}