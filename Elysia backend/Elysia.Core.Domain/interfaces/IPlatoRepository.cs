using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Domain.interfaces
{
    public interface IPlatoRepository : IGenericRepository<Plato>
    {
        Task<List<Plato?>> GetAllByPropietarioId(string propietarioId);

    }
}
