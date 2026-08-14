

using Elysia.Core.Domain.Common;

namespace Elysia.Core.Application.Dtos.Tarjeta
{
    public class EditTarjetaDto
    {
        public required string UsuarioId { get; set; }
        public required string NombreTitular { get; set; }
        public required string NumeroTarjeta { get; set; }
        public required string CVV { get; set; }
        public required int MesVencimiento { get; set; }
        public required int AnioVencimiento { get; set; }
        public required TipoTarjeta Tipo { get; set; }
        public required DateTime FechaRegistro { get; set; }

    }
}
