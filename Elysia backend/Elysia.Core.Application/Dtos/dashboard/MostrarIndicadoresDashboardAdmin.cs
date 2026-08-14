using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.dashboard
{
    public class MostrarIndicadoresDashboardAdmin
    {

        public string AdminId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int CantidadTarjetaVisa {  get; set; }
        public int CantidadTarjetaMastercard { get; set; }
        public int CantidadTarjetaAmericanExpress { get; set; }
        public int CantidadTotalTarjeta {  get; set; }
        public int CantidadAdmin {  get; set; }
        public int CantidadPropietario { get; set; }
        public int CantidadPlanes { get; set; }
        public int MembresiasActivas { get; set; }
        public int MembresiasCanceladas { get; set; }
        public int MembresiaSuspendida {  get; set; }
        public int MembresiaVencida {  get; set; }
        public int TotalMembresias { get; set; }


    }
}
