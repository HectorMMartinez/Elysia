using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IPlatoMenuRepository : IGenericRepository<PlatoMenu>
    {

        Task<List<PlatoMenu?>> AddRangeAsync(List<PlatoMenu> platoMenus);
        Task<List<PlatoMenu?>> GetListByMenuId(int menuId);

    }
}
