using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IReservasRepository : IGenericRepository<Reserva>
    {
        Task<List<Reserva?>> GetReservasActivasByPropietario(string propietario);
        Task<List<Reserva?>> GetReservasFinalizadasByPropietario(string propietario);
        Task<List<Reserva?>> GetReservasCanceladaByPropietario(string propietario);
        Task<List<Reserva?>> GetReservasNoAsistioByPropietario(string propietario);
        Task<List<Reserva?>> GetReservasEnProcesoByPropietario(string propietario);
        Task<List<Reserva?>> GetAllReservasByPropietario(string propietario);
        Task<List<Reserva?>> GetAllReservasByMesaId(int mesaId);
        Task<bool> CambiarEstadoAsync(int id, EstadoReserva estado);
        Task<List<Reserva>> GetReservasByPropietarioAndFecha(string propietarioId, DateTime fechaDesde, DateTime? fechaHasta = null);


    }
}
