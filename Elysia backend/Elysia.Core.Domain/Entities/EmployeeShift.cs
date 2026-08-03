using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class EmployeeShift
    {
        public int Id { get; set; }

        public int EmpleadoId { get; set; }

        public Empleado? Empleado { get; set; } = null;

        public int ShiftId { get; set; }

        public Shift? Shift { get; set; } = null;

        public DateOnly WorkDate { get; set; }
    }
}
