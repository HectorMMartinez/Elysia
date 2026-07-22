using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{
    public class CreatePedidoDto
    {
        public int Id { get; set; }
        public string IdPropietario { get; set; } = string.Empty;
        public int IdMesa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public EstadoPedido Estado { get; set; }
        public List<CreateDetallesPedidoRequestDto> DetallesPedidoDtos { get; set; } = new List<CreateDetallesPedidoRequestDto>();

    }
}
