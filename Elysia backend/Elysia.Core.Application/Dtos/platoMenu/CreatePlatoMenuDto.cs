using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Dtos.platoMenu
{
    public class CreatePlatoMenuDto
    {
        public int Id { get; set; }
        public int IdPlato { get; set; }
        public int IdMenu { get; set; }
    }
}

