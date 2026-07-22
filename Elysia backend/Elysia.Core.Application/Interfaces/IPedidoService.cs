using Elysia.Core.Application.Dtos.pedido;
using Elysia.Core.Domain.Entities;


namespace Elysia.Core.Application.Interfaces
{
    public interface IPedidoService : IGenericService<Pedido,MostrarPedidoConPlatosDto,EditarPedidoDto, CreatePedidoDto>
    {
        Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosPendienteAsync(string propietarioId);
        Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosEntregadoAsync(string propietarioId);
        Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosCanceladosAsync(string propietarioId);
        Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosListoAsync(string propietarioId);
        Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosEnProcesoAsync(string propietarioId);
        Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosFinalizado(string propietarioId);
        Task<MostrarPedidoConPlatosDto> GetPedidoConPlatosByPedidoId(int pedidoId);
        Task<List<MostrarPedidoConPlatosDto>> GetPedidoConPlatosByPropietarioId(string propietarioId);
    }
}
