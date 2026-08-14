using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.movimientoInventario
{
    public class CrearMovimientoInventarioResponseDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public TipoMovimientoInventario TipoMovimiento { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

    }
}
