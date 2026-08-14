using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.producto
{
    public class EditarProductoDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public required string UnidadMedida { get; set; }
        public required decimal StockActual { get; set; }
        public string? Imagen { get; set; }
        public required decimal StockMinimo { get; set; }
        public required bool Activo { get; set; }
        public string? IdPropietario { get; set; }
    }
}



