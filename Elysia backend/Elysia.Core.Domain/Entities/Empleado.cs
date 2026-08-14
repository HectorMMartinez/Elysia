using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Empleado
    {
      
            public int Id { get; set; }

            public string FirstName { get; set; } = string.Empty;

            public string LastName { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public string Phone { get; set; } = string.Empty;

            public DateOnly HireDate { get; set; }

            public decimal Salary { get; set; }

            public bool IsActive { get; set; }

            public string RestaurantId { get; set; } = string.Empty;

  
            public int PuestoId { get; set; }

            public Puesto? Puesto { get; set; } = null;

            public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
        
    }
}
