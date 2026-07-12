using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class PlatoMenu 
    {
        public int Id { get; set; }
        public int IdPlato { get; set; }
        public Plato Plato { get; set; } = new Plato();
        public int IdMenu { get; set; }
        public Menu Menu { get; set; } = new Menu();
       
    }
}
