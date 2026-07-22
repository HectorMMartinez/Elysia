using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IPedidoRepository : IGenericRepository<Pedido>
    {

        Task<List<Pedido>> GetPedidosByPropietarioId(string propietarioId);
        Task<List<Pedido>> GetAllPedidosPendiente(string propietarioId); 
        Task<List<Pedido>> GetAllPedidosEntregado(string propietarioId); 
        Task<List<Pedido>> GetAllPedidosCancelados(string propietarioId); 
        Task<List<Pedido>> GetAllPedidosListo(string propietarioId); 
        Task<List<Pedido>> GetAllPedidosEnProceso(string propietarioId); 
        Task<List<Pedido>> GetAllPedidosByMesaId(int mesaId);
        Task<List<Pedido>> GetAllPedidosFinalizado(string propietarioId);


    }
}
