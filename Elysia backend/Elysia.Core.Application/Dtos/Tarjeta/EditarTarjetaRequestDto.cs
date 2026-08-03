using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.Tarjeta
{
    public class EditarTarjetaRequestDto
    {

        [Required(ErrorMessage = "Debes indicar el nombre del titular para editar")]
        public required string NombreTitular { get; set; }
        [Required(ErrorMessage = "Debes indicar el numero de la tarjeta")]
        public required string NumeroTarjeta { get; set; }
        [Required(ErrorMessage = "Debes indicar el cvv de la tarjeta para editar")]
        public required string CVV { get; set; }
        [Range(1,12,ErrorMessage = "Debes indicar el mes de vencimiento correctamente")]
        public required int MesVencimiento { get; set; }
        [Range(2026, 2050, ErrorMessage = "Debes ingresar un anio valido")]
        public required int AnioVencimiento { get; set; }
        [Required(ErrorMessage = "Debes indicar el tipo de tarjeta correctamente")]
        public required TipoTarjeta Tipo { get; set; }
   
    }
}
