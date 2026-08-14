using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.dashboard
{
    public class MostrarIndicadoresPanelPropietarioSimpleDto
    {

        public string PropietarioId { get; set; } = string.Empty;
        public string NamePropietario { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int PlanId { get; set; }
        public int ReservasActivas { get; set; }
        public int ReservasCanceladas { get; set; }
        public int ReservasFinalizadas { get; set; }
        public int ReservasNoAsistio { get; set; }
        public int ReservaEnProceso { get; set; }
        public int TotalReservas { get; set; }
        public int PedidosListo { get; set; }
        public int PedidosFinalizado { get; set; }
        public int PedidosEntregado { get; set; }
        public int PedidosCancelados { get; set; }
        public int PedidosEnProceso { get; set; }
        public int PedidoPendiente { get; set; }
        public int TotalPedidos { get; set; }
        public int CantidadProducto { get; set; }
        public int CantidadPlato { get; set; }
        public int CantidadMesa {  get; set; }
        public int MesasDisponibles { get; set; }
        public int MesasOcupadas { get; set; }
        public int MesasReservadas { get; set; }
        public int CantidadMenu { get; set; }
        public int PlatoAsociadoAUnMenu { get; set; }

    }
}
