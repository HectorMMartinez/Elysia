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
    public class DetallesPedidoRepository : GenericRepository<DetallesPedido>, IDetallesPedidoRepository
    {
        private readonly ElysiaContext context;

        public DetallesPedidoRepository(ElysiaContext appContext) : base(appContext)
        {
            this.context = appContext;
        }

        public async  Task<List<DetallesPedido>> AddRangeAsync(List<DetallesPedido> detallesPedidos)
        {
            if(detallesPedidos == null || detallesPedidos.Count == 0)
            {
                return [];
            }

            await context.Set<DetallesPedido>().AddRangeAsync(detallesPedidos); 
            await context.SaveChangesAsync();   
            return detallesPedidos;

        }



        public async Task<List<DetallesPedido>> UpdateRangeAsync(
        int pedidoId,
        List<DetallesPedido> detallesPedido)
        {
            if (detallesPedido == null || !detallesPedido.Any())
            {
                return [];
            }

          
            var detallesActuales = await context.Set<DetallesPedido>()
                .Where(x => x.PedidoId == pedidoId)
                .ToListAsync();

         
            if (detallesActuales.Any())
            {
                context.Set<DetallesPedido>().RemoveRange(detallesActuales);
            }

    
            await context.Set<DetallesPedido>().AddRangeAsync(detallesPedido);

            await context.SaveChangesAsync();

        
            return await context.Set<DetallesPedido>()
                .Include(x => x.Plato)
                .Where(x => x.PedidoId == pedidoId)
                .ToListAsync();
        }





        public async Task<List<DetallesPedido>> GetAllByPedidoId(int pedidoId)
        {
            var data = await context.Set<DetallesPedido>().Where(x => x.PedidoId == pedidoId).ToListAsync();

            if (data.Count > 0)
            {
                return data;
            }


            return [];
           
        }


    }
}
