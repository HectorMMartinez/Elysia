using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{

    public class EditarPedidoRequestDto
    {

        //id de la mesa del pedido opciona
        
        public int IdMesa { get; set; }
        //detalles del pedido listado de platos con su cantidad y precio (cambiar opcional)
        public List<CreateDetallesPedidoRequestDto> DetallesPedido { get; set; } = new List<CreateDetallesPedidoRequestDto>();

        [Required(ErrorMessage = "Debes indicar el estado del pedido")] // obligatorio
        public EstadoPedido Estado { get; set; }


    }
}
