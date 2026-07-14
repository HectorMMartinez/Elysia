using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IPlatoProductoRepository : IGenericRepository<PlatoProducto>
    {


        Task<List<PlatoProducto>> AddRangeAsync(List<PlatoProducto> platoProductos);
        Task<List<PlatoProducto?>> GetByPlatoId(int productId);
        Task<List<PlatoProducto>> UpdateRangeAsync(int platoId, List<PlatoProducto> platoProductos);


    }
}
