using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Common
{
    public class MesaEstadisticaDto
    {
        public int IdMesa { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public MesaEstado Estado { get; set; }
        public int Cantidad { get; set; }

    }
}
