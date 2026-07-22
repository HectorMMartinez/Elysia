using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{
    public class CreateDetallesPedidoRequestDto
    {
        public int PlatoId { get; set; }
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }

    }
}
