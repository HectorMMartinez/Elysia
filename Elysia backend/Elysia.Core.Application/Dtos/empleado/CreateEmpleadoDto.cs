using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.empleado
{
    public class CreateEmpleadoDto
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


    }
}
