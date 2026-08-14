using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IGenericService<Tentity,Tresponse,TeditDto,TaddDto> 
    where Tentity : class where Tresponse : class where TeditDto : class where TaddDto : class
    {
        Task<Tresponse?> AddAsync(TaddDto? entity);
        Task<Tresponse?> UpdateAsync(int id, TeditDto? entity);
        Task<List<Tentity?>> GetlAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<Tentity?> GetByIdAsync(int id);

    }
}
