using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.plato
{
    public class CreatePlatoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string IdPropietario { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public PlatoEstado Estado { get; set; }
        public List<productoQuantityDto> ProductoQuantityDtos { get; set; } = new List<productoQuantityDto>();

    }
}
