using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        Task<List<Producto>> GetByIdsAsync(IEnumerable<int> ids);
        Task<List<Producto>?> UpdateRangeAsync(List<Producto> products);

    }
}
