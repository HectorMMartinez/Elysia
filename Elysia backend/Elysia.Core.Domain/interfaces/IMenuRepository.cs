

using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Core.Domain.interfaces
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {

        Task<List<Menu?>> GetListMenuByPropietarioId(string propietarioId); 


    }
}
