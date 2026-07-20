using Elysia.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.menu
{
    public class MenuResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string IdPropietario { get; set; } = string.Empty;
        public MenuEstado Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool IsPrincipal { get; set; }
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();


    }
}
