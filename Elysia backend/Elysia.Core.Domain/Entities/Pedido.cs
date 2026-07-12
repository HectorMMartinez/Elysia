using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Pedido 
    {
        public int Id { get; set; } 
        public string IdPropietario { get; set; } = string.Empty;
        public int IdMesa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public EstadoPedido Estado {  get; set; }




        //navigation property
        public Mesa Mesa { get; set; } = new Mesa();
        public ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();  

    }
}
