using Elysia.Core.Domain.Common;
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
    public class PedidoRepository : GenericRepository<Pedido>, IPedidoRepository
    {

        private readonly ElysiaContext context; 


        public PedidoRepository(ElysiaContext appContext) : base(appContext)
        {
            this.context = appContext;
        }

        public async  Task<List<Pedido>> GetAllPedidosByMesaId(int mesaId)
        {
            var data = await context.Set<Pedido>().Where(x => x.IdMesa == mesaId && x.Estado == EstadoPedido.Pendiente).ToListAsync();

            if (data.Count > 0){
               
                return data;
            
            }

            return [];
        }


    
      
        public async  Task<List<Pedido>> GetAllPedidosCancelados(string propietarioId)
        {

            var data = await context.Set<Pedido>().Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Cancelado).ToListAsync();

            if (data.Any()) {
               
                return data;
            }

            return new List<Pedido>();
        }

        public async Task<List<Pedido>> GetAllPedidosEnProceso(string propietarioId)
        {
           var data = await context.Set<Pedido>().Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.EnPreparacion).ToListAsync();

            if (data.Any())
            {
                return data;
            
            }

            return data;

        }



        public async Task<List<Pedido>> GetAllPedidosFinalizado(string propietarioId)
        {
            var data = await context.Set<Pedido>().Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Finalizado).ToListAsync();

            if (data.Any())
            {
                return data;

            }

            return new List<Pedido>();

        }



        public async Task<List<Pedido>> GetAllPedidosEntregado(string propietarioId)
        {
            var data = await context.Set<Pedido>().Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Entregado).ToListAsync();

            if (data.Any())
            {
                return data;
            }

            return [];
        }



        public async Task<List<Pedido>> GetAllPedidosListo(string propietarioId)
        {
            var data = await context.Set<Pedido>().Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Listo).ToListAsync();

            if (data.Any())
            {
                return data;
            }


            return [];
            
        }



        public  async Task<List<Pedido>> GetAllPedidosPendiente(string propietarioId)
        {
            var data = await context.Set<Pedido>().Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Pendiente).ToListAsync();

            if (data.Any())
            {
                return data;
            }

            return [];
        }




        public  async Task<List<Pedido>> GetPedidosByPropietarioId(string propietarioId)
        {
            var data = await context.Set<Pedido>().Where(x => x.IdPropietario == propietarioId).ToListAsync();

            if (data.Any())
            {

                return data;

            }

            return data;
           
        }


        //cambiar el estado de un pedido
        public async Task<bool> CambiarEstadoAsync(int id, EstadoPedido estado)
        {
            var pedido = await context.Set<Pedido>().FindAsync(id);

            if (pedido != null)
            {
                pedido.Estado = estado;
                await context.SaveChangesAsync();
                return true;
            }


            return false;
        }


      public async Task<List<Pedido>> GetPedidosByPropietarioAndFecha(string propietarioId, DateTime fechaDesde,DateTime? fechaHasta = null)
        {
            var query = context.Pedidos
                .AsNoTracking()
                .Include(p => p.DetallesPedidos)
                    .ThenInclude(d => d.Plato)          // importante para sacar el nombre del plato
                .Where(p => p.IdPropietario == propietarioId
                         && p.FechaCreacion >= fechaDesde);

            if (fechaHasta.HasValue)
            {
                query = query.Where(p => p.FechaCreacion <= fechaHasta.Value);
            }

            return await query
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }
    }
}
