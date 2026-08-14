using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class ForgotPasswordRequestDto
    {
        [Required(ErrorMessage = "Debes introducir  el usuario")]
        public required string Email { get; set; }
    }
}
