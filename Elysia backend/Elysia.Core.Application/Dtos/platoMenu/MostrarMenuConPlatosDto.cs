using Elysia.Core.Application.Dtos.plato;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.platoMenu
{
    public class MostrarMenuConPlatosDto
    {



        public int MenuId { get; set; }
        public string NombreMenu { get; set; }= string.Empty;
        public string DescripcionMenu { get; set; }= string.Empty;  
        public MenuEstado MenuEstado { get; set; }
        public List<CreatePlatoDto> Platos { get; set; } = new List<CreatePlatoDto>();



    }
}
