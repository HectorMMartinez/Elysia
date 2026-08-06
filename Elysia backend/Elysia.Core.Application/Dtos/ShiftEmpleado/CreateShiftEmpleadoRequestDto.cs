using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.ShiftEmpleado
{
    public class CreateShiftEmpleadoRequestDto
    {


        [Range(1,int.MaxValue,ErrorMessage = "Debes seleccionar un empleado valido")]
        public int EmpleadoId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage = "Debes seleccionar un turno valido")]
        public int ShiftId { get; set; }
        [Required]
        public DateOnly WorkDate { get; set; }

    }
}
