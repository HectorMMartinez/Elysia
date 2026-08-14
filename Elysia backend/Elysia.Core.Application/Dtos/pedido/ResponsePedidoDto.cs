using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.pedido
{
    public class ResponsePedidoDto
    {
        public bool? HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
