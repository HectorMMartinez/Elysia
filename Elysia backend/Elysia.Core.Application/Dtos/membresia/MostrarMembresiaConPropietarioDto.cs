using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.membresia
{
    public class MostrarMembresiaConPropietarioDto
    {

        public int Id { get; set; }
        public required string UsuarioId { get; set; }
        public required string UserName {  get; set; }
        public required string NombreRestaurante {  get; set; }
        public required string NombrePlan {  get; set; }
        public required int PlanId { get; set; }
        public required DateTime FechaInicio { get; set; }
        public required DateTime FechaFin { get; set; }
        public required MembresiaEstado Estado { get; set; }
    }

}
