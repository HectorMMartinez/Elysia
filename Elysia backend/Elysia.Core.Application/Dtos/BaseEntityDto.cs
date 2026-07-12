using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos
{
    public class BaseEntityDto
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Debes indicar un nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes indicar una descripcion")]
        public string Descripcion { get; set; } = string.Empty;

    }
}
