using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.CenterIA
{
    public class MensajeChatDto
    {
        public string Rol { get; set; } = string.Empty; // "user" o "assistant"
        public string Contenido { get; set; } = string.Empty;
    }
}

