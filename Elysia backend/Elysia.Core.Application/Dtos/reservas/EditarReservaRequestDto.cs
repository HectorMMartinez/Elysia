using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.reservas
{
    public class EditarReservaRequestDto
    {
        [Required(ErrorMessage = "Debes indicar el nombre del cliente")]
        public string NombreCliente { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debes ingresar el dni del cliente")]
        public string DNICliente { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar la mesa  que sera reservada")]
        public int MesaId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar la cantidad de persona")]
        public int CantidadPersona { get; set; }
        [Required(ErrorMessage = "Debes indicar el estado de la reserva")]
        public EstadoReserva Estado { get; set; }
        [Required(ErrorMessage = "Debes indicar la fecha de la reserva")]
        public DateTime FechaReserva { get; set; }
        public string? Observaciones { get; set; }

    }
}
