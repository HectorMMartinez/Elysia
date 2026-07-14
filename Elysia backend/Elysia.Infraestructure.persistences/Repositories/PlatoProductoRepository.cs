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
    public class PlatoProductoRepository : GenericRepository<PlatoProducto>, IPlatoProductoRepository
    {
        private readonly ElysiaContext context;
        public PlatoProductoRepository(ElysiaContext appContext) : base(appContext)
        {
            context = appContext;
        }



        public async Task<List<PlatoProducto>> AddRangeAsync(List<PlatoProducto> platoProductos)
        {

            if(platoProductos == null || platoProductos.Count == 0)
            {
                return [];
            }
           
            await context.Set<PlatoProducto>().AddRangeAsync(platoProductos);
            await context.SaveChangesAsync();
            return platoProductos;
        }

      

        public async Task<List<PlatoProducto?>> GetByPlatoId(int platoId)
        {
            var entities = await context.Set<PlatoProducto>().Where(x => x.PlatoId == platoId).ToListAsync();

            if(entities == null || entities.Count == 0)
            {
                return new List<PlatoProducto?>();
            }


            return entities;
        }



        public async Task<List<PlatoProducto>> UpdateRangeAsync(int platoId,List<PlatoProducto> platoProductos)
        {
            var dataPlatoProducto = await context.Set<PlatoProducto>().Where(x => x.PlatoId == platoId).ToListAsync();

            if(dataPlatoProducto == null || dataPlatoProducto.Count == 0)
            {
                return [];
            }


            if (platoProductos == null || platoProductos.Count == 0)
            {
                return [];
            }


            context.Set<PlatoProducto>().UpdateRange(platoProductos);
            await context.SaveChangesAsync();
            return platoProductos;
        }






    }
} 
