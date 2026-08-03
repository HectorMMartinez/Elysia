using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.shift
{
    public class CreateShiftRequestDto
    {


        [Required(ErrorMessage = "Debes indicar un nombre correcto para el turno")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public TimeOnly? StartTime { get; set; }
        [Required]
        public TimeOnly? EndTime { get; set; }

    }
}
