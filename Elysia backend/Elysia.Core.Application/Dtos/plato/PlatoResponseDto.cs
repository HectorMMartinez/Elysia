using Elysia.Core.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.plato
{
    public class PlatoResponseDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string? Imagen { get; set; }
        public int CategoriaId { get; set; }
        public PlatoEstado Estado { get; set; }
        public List<productoQuantityDto> ProductoQuantityDtos { get; set; } = new List<productoQuantityDto>();
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

    }
}
