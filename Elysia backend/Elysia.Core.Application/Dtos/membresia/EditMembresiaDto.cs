using Elysia.Core.Domain.Common;


namespace Elysia.Core.Application.Dtos.membresia
{
    public class EditMembresiaDto
    {
        public required string UsuarioId { get; set; }
        public required int PlanId { get; set; }
        public required DateTime FechaInicio { get; set; }
        public required DateTime FechaFin { get; set; }
        public required MembresiaEstado Estado { get; set; }
    }
}
