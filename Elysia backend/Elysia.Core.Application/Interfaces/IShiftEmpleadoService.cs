using Elysia.Core.Application.Dtos.ShiftEmpleado;
using Elysia.Core.Application.Services;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IShiftEmpleadoService : IGenericService<EmployeeShift,ShiftEmpleadoResponseDto,CreateShiftEmpleadoDto,CreateShiftEmpleadoDto>
    {
        Task<List<MostrarShiftEmpleadoConNombreDto>?> MostrarShiftEmpleadoConNombreDtos(string propietarioId);

    }
}
