using APIs.Modelos.Dtos;

namespace APIs.Modelos.Entidade;

public class ExtratoBancario
{
    public string Nome { get; set; } = string.Empty;
    public string Identificador { get; set; } = string.Empty;
    public List<MostrarTransferenciaDto> Transferencias { get; set; }
}