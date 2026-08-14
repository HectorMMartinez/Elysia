using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class PlatoProducto
    {
        public int Id { get; set; }
        public int PlatoId { get; set; }

        public Plato Plato { get; set; } = null!;
        public int ProductoId { get; set; }

        public Producto Producto { get; set; } = null!;
        public decimal Cantidad { get; set; }
    }
}


