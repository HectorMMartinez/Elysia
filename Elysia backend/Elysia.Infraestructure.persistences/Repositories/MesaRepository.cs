using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class MesaRepository : GenericRepository<Mesa>, IMesaRepository
    {
        private readonly ElysiaContext context;

        public MesaRepository(ElysiaContext appContext) : base(appContext)
        {
            context = appContext;
        }


        public async Task<List<Mesa>> GetAllByPropietarioId(string propietarioId)
        {
            var mesas = await context.Set<Mesa>().Where(x => x.IdPropietario == propietarioId).ToListAsync();

            if (mesas.Any())
            {

                return mesas;
            
            }
            return [];   
        }



    }
}
