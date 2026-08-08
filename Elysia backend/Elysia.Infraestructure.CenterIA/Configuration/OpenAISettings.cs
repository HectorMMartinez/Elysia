using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.CenterIA.Configuration
{
    public class OpenAISettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gpt-4.1-mini"; // o el modelo que uses
        public string BaseUrl { get; set; } = "https://api.openai.com/v1/";


    }
}
