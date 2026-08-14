using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Puesto
    {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
        
    }
}
