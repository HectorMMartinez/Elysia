

using System.ComponentModel.DataAnnotations;

namespace Elysia.Core.Application.Dtos.User
{
    public class ConfirmRequestDto
    {
        [Required(ErrorMessage ="Ocurrio un error con el usuario,no se identifico para confirmar la cuenta")]
        public required string UserId { get; set; }
        [Required(ErrorMessage = "Debes introducir el token para confirma la cuenta")]
        public required string Token { get; set; }
    }
}
