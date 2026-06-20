

using Elysia.Core.Domain.Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Elysia.Core.Application.Dtos.User
{
    public class RegisterUserRequestDto
    {

        [Required(ErrorMessage = "Debes ingresar el nombre personal")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Debes ingresar el apellido")]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "debes ingresar el correo")]
        public required string Email { get; set; }
        [Required( ErrorMessage = "Debes ingresar el nombre de usuario")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Debes ingresar la contrasenia")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Debes ingresar tu numero de telefono")]
        public required string? Phone { get; set; } //del usuario

        [Required(ErrorMessage = "Debes ingresar la foto de perfil")]
        public IFormFile? ProfileImage { get; set; } //del usuario
        [Required(ErrorMessage = "Debes ingresar el RNC del negocio")]
        public required string RNC { get; set; }
        [Required(ErrorMessage = "Debes ingresar el nombre del restaurante")]
        public required string NombreRestaurante { get; set; }
        [Required(ErrorMessage = "Debes ingresar el logo del restaurante")]
        public IFormFile? LogoRestaurante { get; set; } //restaurante
        [Required(ErrorMessage = "Debes ingresar la cedula")]
        public required string IdCard { get; set; } //propietario o usuario
        [Required(ErrorMessage = "Debes ingresar la hora de apertura del negocio")]
        public TimeOnly HoraApertura { get; set; }
        [Required(ErrorMessage = "Debes ingresar la hora de apertura de cierre del negocio")]
        public TimeOnly HoraCierre { get; set; }
        [Required(ErrorMessage = "Debes ingresar la direccion del restaurante")]
        public required string DireccionRestaurante { get; set; }
        [Required(ErrorMessage = "Debes ingresar el phone del restaurante")]
        public required string PhoneRestaurante { get; set; } //restaurante
        [Required(ErrorMessage = "Debes ingresar la especialidad del restaurante")]
        public required string Especialidad { get; set; }

        [Range(1,2,ErrorMessage = "Debes escoger un plan")]
        public required int PlanId { get; set; } //id del plan que esta en la db

        [Required(ErrorMessage = "Debes ingresar el nombre del titular de la tarjeta")]
        public required string NombreTitular { get; set; } //tarjeta
        [Required(ErrorMessage = "Debes ingresar el numero de la tarjeta")]
        public required string NumeroTarjeta { get; set; }// tarjeta
        [Required(ErrorMessage = "Debes ingresar el CVV de la tarjeta")]
        public required string CVV { get; set; } //tarjeta
        [Range(1,12,ErrorMessage = "Debes ingresar un mes valido")]
        public required int MesVencimiento { get; set; } //tarjeta
        [Range(2026,2050,ErrorMessage = "Debes ingresar un anio valido")]
        public required int AnioVencimiento { get; set; } //tarjeta
        [Required(ErrorMessage = "Debes ingresar un tipo de tarjeta valida")]
        public required TipoTarjeta Tipo { get; set; }




    }
}
