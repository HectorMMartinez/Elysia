using Elysia.Core.Application.Dtos.dashboard;

namespace Elysia.Core.Application.Interfaces
{
    public interface IDashboardPropietarioServices
    {
        Task<(MostrarIndicadoresPanelPropietarioPremiumDto?, MostrarIndicadoresPanelPropietarioSimpleDto?)> GetIndicadoresPanelPropietario(string PropietarioId);
    }
}