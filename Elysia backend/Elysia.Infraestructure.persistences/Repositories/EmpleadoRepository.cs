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
    public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleadoRepository
    {
        private readonly ElysiaContext _context;   


        public EmpleadoRepository(ElysiaContext appContext) : base(appContext)
        {
            this._context = appContext;
        }

        public  async Task<List<Empleado>?> GetAllEmpleadoByPropietarioId(string propietarioId)
        {
            var data = await _context.Set<Empleado>().Where(x => x.RestaurantId == propietarioId).ToListAsync();

            if (data.Any())
            {
                return data;
            }

            return new List<Empleado>();
        }

        public async Task<List<Empleado>?> GetAllEmpleadosActivos(string restauranteId)
        {
            var data = await _context.Set<Empleado>().Where(x => x.RestaurantId == restauranteId && x.IsActive == true).ToListAsync();
            if (data.Any())
            {
                return data;
            }

            return new List<Empleado>();
        }



        public  async Task<List<Empleado>?> GetAllEmpleadosInactivo(string restauranteId)
        {
            var data = await _context.Set<Empleado>().Where(x => x.RestaurantId == restauranteId && !x.IsActive).ToListAsync();
            if (data.Any())
            {
                return data;
            
            }

            return new List<Empleado>();

        }
    }
}
