using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.plan
{
    public class SavePlanDto
    {
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required decimal PrecioMensual { get; set; }
    }
}
