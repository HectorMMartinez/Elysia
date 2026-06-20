using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.Tarjeta
{
    public class TarjetaResponseDto
    {

        public  int  Id { get; set; }
        public  string UsuarioId { get; set; }
        public  string NombreTitular { get; set; }
        public  string NumeroTarjeta { get; set; }
        public  string CVV { get; set; }
        public  int MesVencimiento { get; set; }
        public  int AnioVencimiento { get; set; }
        public  TipoTarjeta Tipo { get; set; }
        public  DateTime FechaRegistro { get; set; }
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; }
        


    }
}
