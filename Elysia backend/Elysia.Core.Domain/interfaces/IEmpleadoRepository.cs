using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IEmpleadoRepository : IGenericRepository<Empleado>
    {
        Task<List<Empleado>?> GetAllEmpleadosActivos(string restauranteId);
        Task<List<Empleado>?> GetAllEmpleadosInactivo(string restauranteId);
        Task<List<Empleado>?> GetAllEmpleadoByPropietarioId(string propietarioId);
       



    }
}
