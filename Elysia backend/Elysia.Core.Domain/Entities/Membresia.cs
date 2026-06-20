using Elysia.Core.Domain.Common;



namespace Elysia.Core.Domain.Entities
{
    public class Membresia
    {


        public int Id { get; set; }
        public required string UsuarioId { get; set; }
        public Plan? Plan { get; set; }
        public required int PlanId { get; set; }
        public required DateTime FechaInicio { get; set; }
        public required DateTime FechaFin {  get; set; }
        public required MembresiaEstado Estado { get; set; }


    }
}
