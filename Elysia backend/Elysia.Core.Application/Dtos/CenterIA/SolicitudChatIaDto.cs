using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.CenterIA
{
    public class SolicitudChatIaDto
    {
        public string Pregunta { get; set; } = string.Empty;

        /// <summary>
        /// Historial de la conversación (opcional).
        /// El frontend envía los mensajes anteriores.
        /// </summary>
        public List<MensajeChatDto>? Historial { get; set; } = new();
    }
}
