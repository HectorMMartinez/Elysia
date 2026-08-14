using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Mesa : BaseEntity
    {
        //hereda propiedas que va autilizar desde baseEntity
        public string IdPropietario { get; set; } = string.Empty;
        public MesaEstado Estado { get; set; }
        public int Capacidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        
        //navitagion property
        public ICollection<Reserva> Reservas { get; set; }  = new List<Reserva>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
