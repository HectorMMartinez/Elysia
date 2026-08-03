using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class EditPerfilUserDto
    {
        [Required(ErrorMessage = "Debes ingresar el nombre personal")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Debes ingresar el apellido")]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "debes ingresar el correo")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Debes ingresar el nombre de usuario")]
        public required string UserName { get; set; }


        //opcional al editar
        public required string Password { get; set; }
        //opcional al editar
        public required string? Phone { get; set; } //del usuario

        //opcional al editar
        public IFormFile? ProfileImage { get; set; } //del usuario
      
    }
}
