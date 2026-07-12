

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Elysia.Infraestructure.Identity.Entities
{
    public class AppUser :  IdentityUser
    {

        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string ProfileImage { get; set; }
        public  string? NombreRestaurante { get; set; }
        public  string? LogoRestaurante { get; set; }
        public required bool IsActive { get; set; }
        public  string? IdCard { get; set; }
        public string? RNC { get; set; } = string.Empty;
        public TimeOnly? HoraApertura { get; set; }
        public TimeOnly? HoraCierre { get; set; }
        public  string? DireccionRestaurante { get; set; } 
        public string? PhoneRestaurante { get; set; }
        public  string? Especialidad {  get; set; }
        


    }
}
