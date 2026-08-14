using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class EditarDatosRestaurante
    {
        //opcional al editar
        public required string RNC { get; set; }
        [Required(ErrorMessage = "Debes ingresar el nombre del restaurante")]
        public required string NombreRestaurante { get; set; }
        //opcional al editar
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
    }
}
