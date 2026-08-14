using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.CenterIA
{
    public class RespuestaOptimizacionMenuDto
    {
        public List<string> PlatosPromocionar { get; set; } = [];

        public List<string> PlatosRevisarPrecio { get; set; } = [];

        public List<string> PlatosRetirar { get; set; } = [];

        public List<string> NuevasSugerencias { get; set; } = [];

        public string Resumen { get; set; } = string.Empty;
    }
}
