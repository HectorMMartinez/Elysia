using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.reservas
{
    public class ReservaResponseDto
    {
        public int Id { get; set; }
        public string IdPropietario { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string DNICliente { get; set; } = string.Empty;
        public int MesaId { get; set; }
        public int CantidadPersona { get; set; }
        public EstadoReserva Estado { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string? Observaciones { get; set; }
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

    }
}
