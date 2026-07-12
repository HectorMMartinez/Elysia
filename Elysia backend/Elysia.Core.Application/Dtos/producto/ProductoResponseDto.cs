using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.producto
{
    public class ProductoResponseDto : BaseEntityDto
    {
        public string? IdPropietario { get; set; }
        public string? UnidadMedida { get; set; }

        public  decimal StockActual { get; set; }
        public  string? Imagen { get; set; } 

        public  decimal StockMinimo { get; set; }

        public  bool Activo { get; set; }

        public  DateTime FechaCreacion { get; set; }
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();


    }
}
