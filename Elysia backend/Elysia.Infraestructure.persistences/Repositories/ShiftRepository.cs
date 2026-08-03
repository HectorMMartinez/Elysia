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
    public class ShiftRepository : GenericRepository<Shift>, IShiftRepository
    {
        private readonly ElysiaContext _context;



        public ShiftRepository(ElysiaContext appContext) : base(appContext)
        {
            this._context = appContext;
        }


        public async Task<List<Shift>?> GetAllTurnoByPropietarioId(string PropietarioId)
        {
            var data = await _context.Set<Shift>().Where(x => x.PropietarioId == PropietarioId).ToListAsync();
            if (data.Any())
            {
                return data;
            
            }

            return new List<Shift>();
        }





    }
}
