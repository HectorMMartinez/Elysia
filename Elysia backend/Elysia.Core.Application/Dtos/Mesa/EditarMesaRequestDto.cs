using Elysia.Core.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.Mesa
{
    public class EditarMesaRequestDto
    {
        [Required(ErrorMessage = "Debes ingresar el nombre de la mesa ")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes ingresar la descripcion de la mesa ")]
        public string Descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes ingresar el estado de la mesa ")]
        public MesaEstado Estado { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debes ingresar la capacidad de la mesa (cantidad de personas)")]
        public int Capacidad { get; set; }
        [Required(ErrorMessage = "Debes ingresar la imagen de la mesa ")]
        public IFormFile? Imagen { get; set; }
    }
}
