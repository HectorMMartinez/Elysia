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

    public class ReservasRepository : GenericRepository<Reserva>, IReservasRepository
    {
        private readonly ElysiaContext context;

        public ReservasRepository(ElysiaContext appContext) : base(appContext)
        {
            this.context = appContext;
        }



        public async Task<List<Reserva?>> GetReservasActivasByPropietario(string propietario)
        {
            var reservas = await context.Set<Reserva>().Where(x => x.IdPropietario == propietario && x.Estado == EstadoReserva.Activa).ToListAsync();


            if (reservas.Any())
            {
               return reservas;
            
            }


            return [];
            
        }


        public async Task<List<Reserva?>> GetReservasCanceladaByPropietario(string propietario)
        {

            var reservas = await context.Set<Reserva>().Where(x => x.IdPropietario == propietario && x.Estado == EstadoReserva.Cancelada).ToListAsync();


            if (reservas.Any())
            {
                return reservas;

            }


            return [];

        }

        public async Task<List<Reserva?>> GetReservasFinalizadasByPropietario(string propietario)
        {

            var reservas = await context.Set<Reserva>().Where(x => x.IdPropietario == propietario && x.Estado == EstadoReserva.Finalizada).ToListAsync();


            if (reservas.Any())
            {
                return reservas;

            }


            return [];
        }


        public async Task<List<Reserva?>> GetReservasNoAsistioByPropietario(string propietario)
        {

            var reservas = await context.Set<Reserva>().Where(x => x.IdPropietario == propietario && x.Estado == EstadoReserva.NoAsistio).ToListAsync();


            if (reservas.Any())
            {
                return reservas;

            }


            return [];
        }

        public async Task<List<Reserva?>> GetAllReservasByPropietario(string propietario)
        {
            var reservas = await context.Set<Reserva>().Where(x => x.IdPropietario == propietario).ToListAsync();

            if (reservas.Any())
            {
                return reservas;
            }

            return [];
        }



        public async Task<List<Reserva?>> GetAllReservasByMesaId(int mesaId)
        {
            var reservas  = await context.Set<Reserva>().Where(x => x.MesaId == mesaId).ToListAsync();

            if (reservas.Any())
            {
                return reservas;
            }


            return [];
           
        }




    }

}
