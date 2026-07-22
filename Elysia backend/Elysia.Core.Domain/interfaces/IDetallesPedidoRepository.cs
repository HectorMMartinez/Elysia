using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IDetallesPedidoRepository : IGenericRepository<DetallesPedido>
    {
        Task<List<DetallesPedido>> GetAllByPedidoId(int pedidoId);
        Task<List<DetallesPedido>> AddRangeAsync(List<DetallesPedido> detallesPedidos);
        Task<List<DetallesPedido>> UpdateRangeAsync(int id, List<DetallesPedido> detallesPedidos);



    }
}
