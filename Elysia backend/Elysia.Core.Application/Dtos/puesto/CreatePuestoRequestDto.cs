using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.puesto
{
    public class CreatePuestoRequestDto
    {

        [Required(ErrorMessage = "Debes ingresar el nombre para agregar el puesto")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes ingresar la descripcion para agregar el puesto")]
        public string Description { get; set; } = string.Empty;
    }
}
