

namespace Elysia.Core.Application.Dtos.User
{
    public class LoginResponseDto
    {

        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public required string Rol { get; set; }
        public bool HasError { get; set; }
        public List<string>? Errors { get; set; }
        public required string Token { get; set; }

    }
}
