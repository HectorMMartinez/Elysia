using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Debes ingresar el correo")]
        public required string Email { get; set; }
        [Required(ErrorMessage  = "Debes ingresar la contrasenia")]
        public required string Password { get; set; }
    }
}
