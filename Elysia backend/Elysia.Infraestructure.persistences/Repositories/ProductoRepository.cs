using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        private readonly ElysiaContext context; 
         
        public ProductoRepository(ElysiaContext appContext) : base(appContext)
        {
            this.context = appContext;  
        }

        public async  Task<List<Producto>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var products  = new List<Producto>();

            foreach (var id in ids)
            {
                var product= await context.Set<Producto>().FindAsync(id);
                products.Add(product);
            }

            return products;
        }
    }
}
