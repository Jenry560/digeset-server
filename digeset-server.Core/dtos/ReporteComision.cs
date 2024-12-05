using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace digeset_server.Core.dtos
{
    public class ReporteComisionDto
    {
        public double TotalComision { get; set; }

        public List<MultaDto> Multas { get; set; } = new List<MultaDto>();
    }
}
