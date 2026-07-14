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
    public class PlatoRepository : GenericRepository<Plato>, IPlatoRepository
    {
        private readonly ElysiaContext context;

        public PlatoRepository(ElysiaContext appContext) : base(appContext)
        {

            this.context = appContext;

        }

        public async Task<List<Plato?>> GetAllByPropietarioId(string propietarioId)
        {
            var data = await context.Set<Plato>().Where(x => x.IdPropietario == propietarioId).ToListAsync();

            if (!data.Any())
            {
                return [];
            }

            return data;
        }



    }
}
