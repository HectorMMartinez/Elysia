using Elysia.Core.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.plato
{
    public class CreatePlatoRequestDto 
    {

        [Required(ErrorMessage = "Debes indicar un nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes indicar una descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Range(1,int.MaxValue,ErrorMessage = "Debes ingresar un precio valido")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "Debes ingresar una imagen valida")]
        public IFormFile? Imagen { get; set; } 
        [Range(1,int.MaxValue,ErrorMessage = "Debes ingresar una categoria de plato valida")]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "Debes ingresar un estado de plato valido")]
        public PlatoEstado Estado { get; set; }

        [Required(ErrorMessage = "Debes ingresar productos validos para crear el plato")]
        public List<productoQuantityDto> ProductoQuantityDtos { get; set; } = [];



    }
}
