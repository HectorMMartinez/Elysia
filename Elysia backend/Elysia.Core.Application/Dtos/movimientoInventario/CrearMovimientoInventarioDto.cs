using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.movimientoInventario
{
    public class CrearMovimientoInventarioDto
    {

        public int ProductoId { get; set; }
        public TipoMovimientoInventario TipoMovimiento { get; set; }
        public decimal Cantidad { get; set; }
    }
}
