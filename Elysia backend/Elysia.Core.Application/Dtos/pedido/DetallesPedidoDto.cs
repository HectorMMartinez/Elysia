using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{
    public class DetallesPedidoDto
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }
        public int PlatoId { get; set; }
   
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public string? Observaciones { get; set; }

    }
}
