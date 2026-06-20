using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class SaveUserRequestDto
    {
       
        public  required string Id { get; set; } 
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string? Phone { get; set; }
        public string? ProfileImage { get; set; }
        public required string Role { get; set; }
        public required string RNC { get; set; }
        public required string NombreRestaurante { get; set; }
        public string? LogoRestaurante { get; set; }
        public required string IdCard { get; set; }
        public TimeOnly HoraApertura { get; set; }
        public TimeOnly HoraCierre { get; set; }
        public required string DireccionRestaurante { get; set; }
        public required string PhoneRestaurante { get; set; }
        public required string Especialidad { get; set; }
    }
}
