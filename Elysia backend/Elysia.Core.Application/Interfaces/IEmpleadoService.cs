using Elysia.Core.Application.Dtos.empleado;
using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IEmpleadoService : IGenericService<Empleado,EmpleadoResponseDto,EditarEmpleadoDto,CreateEmpleadoDto>
    {
        Task<List<EmpleadoResponseDto>> GetAllEmpleadosActivos(string restauranteId);
        Task<List<EmpleadoResponseDto>> GetAllEmpleadosInactivos(string restauranteId);
        Task<List<MostrarEmpleadoConPuestoDto>?> GetAllEmpleadoConPuesto(string restauranteId);

    }
}
