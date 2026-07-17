using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.Mesa
{
    public class CreateMesaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string IdPropietario { get; set; } = string.Empty;
        public MesaEstado Estado { get; set; }
        public int Capacidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
    }
}
