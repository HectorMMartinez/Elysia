using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class ConfirmResponseDto
    {
        public required bool HasError { get; set; }
        public required string Message { get; set; }
    }
}
