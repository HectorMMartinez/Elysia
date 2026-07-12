using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class DetallesPedido
    {
       
            public int Id { get; set; }

            public int PedidoId { get; set; }
            public Pedido Pedido { get; set; } = null!;

            public int PlatoId { get; set; }
            public Plato Plato { get; set; } = null!;

            public int Cantidad { get; set; }

            public decimal PrecioUnitario { get; set; }

            public string? Observaciones { get; set; }
     
            
    }
}
