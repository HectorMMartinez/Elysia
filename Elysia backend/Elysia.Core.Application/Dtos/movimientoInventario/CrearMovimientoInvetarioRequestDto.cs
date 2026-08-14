using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.movimientoInventario
{
    public class CrearMovimientoInvetarioRequestDto
    {


        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar un producto valido para el movimiento")]
        public int ProductoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes indicar una cantidad valida para el movimiento")]
        public decimal Cantidad { get; set; }
    }
}
