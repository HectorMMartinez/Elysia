using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Entities
{
    public class Plato : BaseEntity
    {
       
        //hereda algunas propiedades de base entity
        public string IdPropietario { get; set; }  = string.Empty;
        public decimal Precio { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public DateOnly Fecha {  get; set; } 
        public string Codigo { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public PlatoEstado Estado { get; set; }
        
        //property navigation
        public CategoriaPlato Categoria { get; set; } = new CategoriaPlato(); //inicializar
        public ICollection<PlatoMenu> PlatoMenus { get; set; } = new List<PlatoMenu>();
        public ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();
        public ICollection<PlatoProducto> PlatoProductos { get; set; } = new List<PlatoProducto>();
    }

}
