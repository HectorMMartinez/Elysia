using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.CenterIA.Models
{
    public class OpenAIContentItem
    {
        public string Type { get; set; } = string.Empty; // "output_text"
        public string Text { get; set; } = string.Empty;

    }
}
