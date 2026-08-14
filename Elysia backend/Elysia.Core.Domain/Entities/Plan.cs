using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required decimal PrecioMensual  { get; set; }

        public ICollection<Membresia>? Membresias = new Collection<Membresia>();
       
    }
}

