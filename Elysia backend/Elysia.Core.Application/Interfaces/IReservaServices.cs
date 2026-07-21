using Elysia.Core.Application.Dtos.reservas;
using Elysia.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IReservaServices : IGenericService<Reserva,ReservaResponseDto,EditarReservaDto,CreateReservaDto>
    {
        Task<List<ReservaResponseDto?>> GetReservasActivasByPropietario(string propietario);
        Task<List<ReservaResponseDto?>> GetReservasFinalizadasByPropietario(string propietario);
        Task<List<ReservaResponseDto?>> GetReservasCanceladaByPropietario(string propietario);
        Task<List<ReservaResponseDto?>> GetReservasNoAsistioByPropietario(string propietario);
        Task<List<ReservaResponseDto?>> GetAllReservasByPropietario(string propietario);
    }
}
