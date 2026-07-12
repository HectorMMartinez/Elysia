using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Menu : BaseEntity
    {
        //hereda las propiedades que va a utilizar 
        public string IdPropietario { get; set; } = string.Empty;
        public MenuEstado Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool IsPrincipal { get; set; }
        public ICollection<PlatoMenu> PlatoMenus { get; set; } = new List<PlatoMenu>();
     
    }
}
