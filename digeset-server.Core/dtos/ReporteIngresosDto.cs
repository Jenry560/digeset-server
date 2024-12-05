
namespace digeset_server.Core.dtos
{
    public partial class ReporteIngresosDto
    {


        public int ConceptoId { get; set; }
        public string? TipoMulta { get; set; }

        public int CantidadMulta { get; set; }

        public double TotalIngresos { get; set; }
    }
}
