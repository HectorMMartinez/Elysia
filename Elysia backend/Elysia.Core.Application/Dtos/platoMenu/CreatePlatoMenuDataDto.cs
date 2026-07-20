using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.platoMenu
{
    public class CreatePlatoMenuDataDto
    {
        [Range(1,int.MaxValue, ErrorMessage = "Debes indicar el menu para agregar platos")]
        public required int IdMenu { get; set; }
        [Required(ErrorMessage = "Debes indicar platos validos para agregar al menu")]
        public required List<int> PlatoIds { get; set; } = new List<int>();

    }
}
