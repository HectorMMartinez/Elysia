using Elysia.Core.Application.Dtos.menu;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IMenuService : IGenericService<Menu,MenuResponseDto,EditarMenuDto,CreateMenuDto>
    {
    }
}
