using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.producto
{
    public class SaveProductoDto
    {
        [Required(ErrorMessage = "Debes indicar el nombre del producto")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes indicar una descripcion para el producto")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes indicar la unidad de medida del producto")]
        public required string UnidadMedida { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar el stock actual del producto")]
        public required decimal StockActual { get; set; }
        [Required(ErrorMessage = "Debes indicar una imagen para el producto ")]
        public IFormFile? Imagen { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar el stock minimo")]
        public required decimal StockMinimo { get; set; }



    }
}
