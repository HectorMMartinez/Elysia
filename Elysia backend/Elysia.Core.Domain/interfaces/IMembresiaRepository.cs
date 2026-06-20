
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Core.Domain.interfaces
{
    public interface IMembresiaRepository : IGenericRepository<Membresia>
    {

        Task<bool> CambiarEstado(int id, MembresiaEstado estado);


    }
}
