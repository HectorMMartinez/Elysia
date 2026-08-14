using Elysia.Core.Application.Dtos.dashboard;

namespace Elysia.Core.Application.Interfaces
{
    public interface IDashboardAdminService
    {
        Task<MostrarIndicadoresDashboardAdmin> GetPanelAdmin(string adminId);
    }
}