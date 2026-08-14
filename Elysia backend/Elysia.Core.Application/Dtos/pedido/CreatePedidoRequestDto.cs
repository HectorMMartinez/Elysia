using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{
    public class CreatePedidoRequestDto
    {


        //id de la mesa del pedido
        [Range(1, int.MaxValue,ErrorMessage = "Debes indicar la mesa para el pedido")]
        public int IdMesa { get; set; }
        //detalles del pedido listado de platos con su cantidad y precio
        [Required(ErrorMessage = "Debes indicar el o los platos  del pedido")]
        public List<CreateDetallesPedidoRequestDto> DetallesPedido { get; set; } = new List<CreateDetallesPedidoRequestDto>();



    }
}
