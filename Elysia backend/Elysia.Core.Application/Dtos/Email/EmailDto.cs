using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.Email
{
    public class EmailDto
    {

        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string HtmlBody { get; set; }
        public  List<string> ToRange { get; set; } = new List<string>();

    }
}
