using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.Common
{
    public enum EstadoReserva
    {
        Activa = 1,
        EnProceso = 2,
        Finalizada = 3,
        Cancelada = 4,
        NoAsistio = 5
    }
}
