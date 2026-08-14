using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class MovimientoInventario
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public Producto Producto { get; set; } = null!;

        public TipoMovimientoInventario TipoMovimiento { get; set; }

        public decimal Cantidad { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
