

using System.ComponentModel.DataAnnotations;

namespace Elysia.Core.Application.Dtos.User
{
    public class RessetPasswordRequestDto
    {

        [Required(ErrorMessage = "Se debe indicar el usuario, para resetear la contrasenia")]
        public required string Id { get; set; }
        [Required(ErrorMessage = "Se debe indicar la contrasenia para resetear")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Se debe indicar el token, para reseatear la contrasenia")]
        public required string Token { get; set; }


    }
}
