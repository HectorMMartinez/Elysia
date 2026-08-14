using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.platoMenu
{
    public class MostrarMenuConPlatosListDto
    {
        public  List<MostrarMenuConPlatosDto> MostrarMenuConPlatosDtos {  get; set; } = new List<MostrarMenuConPlatosDto>();   
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
