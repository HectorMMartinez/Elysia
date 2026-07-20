using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.menu
{
    public class CreateMenuRequestDto 
    {

        [Required(ErrorMessage = "Debes indicar un nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes indicar una descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes indicar el estado del menu")]
        public MenuEstado Estado { get; set; }
        [Required(ErrorMessage = "Debes indicar si el menu es principal o no")]
        public bool IsPrincipal { get; set; }


    }
}
