using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.CenterIA.Models
{
    public class OpenAIOutputItem
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "message", "reasoning", etc.
        public string? Role { get; set; }
        public string? Status { get; set; }
        public List<OpenAIContentItem>? Content { get; set; }

    }
}
