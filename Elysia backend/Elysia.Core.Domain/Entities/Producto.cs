using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Producto : BaseEntity
    {

        public  required string IdPropietario { get; set; }
        public required string UnidadMedida { get; set; }

        public required decimal StockActual { get; set; }
        public required string Imagen {  get; set; }

        public required decimal StockMinimo { get; set; }

        public required bool Activo { get; set; }

        public required DateTime FechaCreacion { get; set; }


        //navitation property
        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();

        public ICollection<PlatoProducto> PlatoProductos { get; set; }= new List<PlatoProducto>();



    }
}
