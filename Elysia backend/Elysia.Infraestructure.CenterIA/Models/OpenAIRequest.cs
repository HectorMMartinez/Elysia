using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.CenterIA.Models
{
    public class OpenAIRequest
    {
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Puede ser un string simple o una lista de mensajes.
        /// Para nuestro caso usaremos string (el prompt completo).
        /// </summary>
        public string Input { get; set; } = string.Empty;

        /// <summary>
        /// Equivalente al system prompt (recomendado por OpenAI en Responses API)
        /// </summary>
        public string? Instructions { get; set; }

        public double? Temperature { get; set; }

    }
}
