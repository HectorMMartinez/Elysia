using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        private readonly ElysiaContext elysiaContext;

        public MenuRepository(ElysiaContext appContext) : base(appContext)
        {
            this.elysiaContext = appContext;
        }

        public async Task<List<Menu?>> GetListMenuByPropietarioId(string propietarioId)
        {
            var data = await elysiaContext.Set<Menu>().Where(x => x.IdPropietario == propietarioId).ToListAsync();

            if (!data.Any()) 
            {
               return new List<Menu?> { null };
            
            }

            return data;

        }


    }
}
