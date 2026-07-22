using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{
    public class MostrarPedidoConPlatosDto
    {

        public int Id { get; set; }//idPedido
        public int IdMesa { get; set; }//id de la mesa
        public string? NombreMesa { get; set; }
        public decimal? TotalPedido {  get; set; }
        public EstadoPedido Estado { get; set; }
        public List<MostrarDetallesPedidoDto> MostrarDetalles { get; set; } = new List<MostrarDetallesPedidoDto>();
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
