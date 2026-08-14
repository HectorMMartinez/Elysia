using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.CenterIA.Models
{
    public class OpenAIResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Object { get; set; } = string.Empty;
        public long CreatedAt { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public List<OpenAIOutputItem> Output { get; set; } = new();

        /// <summary>
        /// Helper para obtener fácilmente el texto de la respuesta
        /// (equivalente a response.output_text del SDK oficial)
        /// </summary>
        public string OutputText =>
            Output?
                .Where(o => o.Type == "message")
                .SelectMany(o => o.Content ?? new())
                .Where(c => c.Type == "output_text")
                .Select(c => c.Text)
                .FirstOrDefault() ?? string.Empty;

    }
}
