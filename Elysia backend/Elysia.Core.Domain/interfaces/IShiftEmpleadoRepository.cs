using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IShiftEmpleadoRepository : IGenericRepository<EmployeeShift>
    {
        Task<List<EmployeeShift>?> GetEmployeeShiftsByEmpleadoId(int empleadoId);
        Task<EmployeeShift?> GetOneEmployeeShiftsByEmpleadoId(int empleadoId);
        Task<List<EmployeeShift>?> GetEmployeeShiftsByShiftId(int shiftId);


    }
}
