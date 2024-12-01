

namespace digeset_server.Core.dtos;

public partial class AgenteDto
{
    public int AgenteId { get; set; }

    public string? Nombre { get; set; }

    public string? Cedula { get; set; }

    public string? Clave { get; set; }

    public bool Estado { get; set; }

    public int UsuarioId { get; set; }
}