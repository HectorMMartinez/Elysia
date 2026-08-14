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
    public class ShiftEmpleadoRepository : GenericRepository<EmployeeShift>, IShiftEmpleadoRepository
    {
        private readonly ElysiaContext elysiaContext;

        public ShiftEmpleadoRepository(ElysiaContext appContext) : base(appContext)
        {
            this.elysiaContext = appContext;
        }

        public async Task<List<EmployeeShift>?> GetEmployeeShiftsByEmpleadoId(int empleadoId)
        {
            var data = await elysiaContext.Set<EmployeeShift>().Where(x => x.EmpleadoId == empleadoId).ToListAsync();
            if (data.Any())
            {
                return data;
            }

            return new List<EmployeeShift>();
            
        }

        public async Task<List<EmployeeShift>?> GetEmployeeShiftsByShiftId(int shiftId)
        {
            var data = await elysiaContext.Set<EmployeeShift>().Where(x => x.ShiftId == shiftId).ToListAsync();
            if (data.Any())
            {
                return data;
            }

            return new List<EmployeeShift>();
        }

        public async  Task<EmployeeShift?> GetOneEmployeeShiftsByEmpleadoId(int empleadoId)
        {
            var data = await elysiaContext.Set<EmployeeShift>().Where(x => x.EmpleadoId == empleadoId).FirstOrDefaultAsync();

            if(data == null)
            {
                return null;
            }

            return data;
        }
    }
}
