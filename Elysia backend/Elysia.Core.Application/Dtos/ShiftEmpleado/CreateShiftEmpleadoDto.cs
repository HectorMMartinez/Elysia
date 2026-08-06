using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.ShiftEmpleado
{
    public class CreateShiftEmpleadoDto
    {

        public int Id { get; set; }

        public int EmpleadoId { get; set; }

        public int ShiftId { get; set; }

        public DateOnly WorkDate { get; set; }
    }
}
