using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Elysia.Core.Application.Dtos.producto
{
    public class EditarProductoRequestDto
    {
        [Required(ErrorMessage = "Debes indicar el nombre del producto")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes indicar una descripcion para el producto")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes indicar la unidad de medida del producto")]
        public required string UnidadMedida { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar el stock actual del producto")]
        public required decimal StockActual { get; set; }

        // La imagen ahora es opcional al editar.
        public IFormFile? Imagen { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar el stock minimo")]
        public required decimal StockMinimo { get; set; }

        [Required(ErrorMessage = "Debes indicar si el producto sigue activo")]
        public required bool Activo { get; set; }
    }
}