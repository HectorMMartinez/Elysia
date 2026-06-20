using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.User
{
    public class EditResponseDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public List<string>? Roles { get; set; }
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public bool HasError { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();

    }
}
