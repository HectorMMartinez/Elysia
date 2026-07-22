using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{
    public class MostrarDetallesPedidoDto
    {
        public int IdPlato { get; set; }
        public string? NombrePlato { get; set; }
        public decimal? PrecioPlato { get; set; }
        public int CantidaPlato { get; set; }
        public string? Observaciones { get; set; }
    }
}
