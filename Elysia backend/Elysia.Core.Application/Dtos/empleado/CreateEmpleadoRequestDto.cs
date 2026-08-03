using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.empleado
{
    public class CreateEmpleadoRequestDto
    {

        [Required(ErrorMessage = "Debes ingresar el nombre del empleado")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes ingresar el apellido del empleado")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes ingresar el correo del empleado")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes ingresar el telefono del empleado")]
        public string Phone { get; set; } = string.Empty;

        [Range(1,int.MaxValue,ErrorMessage = "Debes agregar un salario valido")]
        public decimal Salary { get; set; }

        [Range(1,int.MaxValue,ErrorMessage = "Debes indicar un puesto valido para el empleado")]
        public int PuestoId { get; set; }

    }
}
