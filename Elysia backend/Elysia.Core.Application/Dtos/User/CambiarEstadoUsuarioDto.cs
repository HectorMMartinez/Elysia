using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class CambiarEstadoUsuarioDto
    {

        [Required(ErrorMessage = "Debes ingresar el id del usuario")]
        public required string UsuarioId { get; set; }
        

    }
}
