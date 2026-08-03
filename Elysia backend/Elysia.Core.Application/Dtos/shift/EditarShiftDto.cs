using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.shift
{
    public class EditarShiftDto
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
        public string PropietarioId { get; set; } = string.Empty;


    }
}
