using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.ShiftEmpleado
{
    public class MostrarShiftEmpleadoConNombreDto
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public int ShiftId { get; set; }
        public string NombreShift {  get; set; } = string.Empty;
        public DateOnly WorkDate {  get; set; }

    }
}
