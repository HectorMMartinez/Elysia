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
    public interface IMesaRepository : IGenericRepository<Mesa>
    {

        Task<List<Mesa>> GetAllByPropietarioId(string propietarioId);
        Task<List<Mesa>> GetAllDisponibleByPropietarioId(string propietarioId);
        Task<List<Mesa>> GetAllDisponibleXByPropietarioId(string propietarioId);
        Task<List<Mesa>> GetAllOcupadasXByPropietarioId(string propietarioId);
        Task<List<Mesa>> GetAllReservadasXByPropietarioId(string propietarioId);
        Task<List<MesaEstadisticaDto>> GetMesasConMasReservasAsync(string restauranteId, DateTime fechaDesde);
        Task<List<MesaEstadisticaDto>> GetMesasConMasPedidosAsync(string restauranteId, DateTime fechaDesde);


    }
}
