using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.shift
{
    public class ShiftResponseDto
    {
    
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

    }
}
