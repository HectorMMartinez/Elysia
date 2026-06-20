

using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class TarjetaRepository : GenericRepository<Tarjeta>,ITarjetaRepository
    {
        public TarjetaRepository(ElysiaContext appContext) : base(appContext)
        {
        }
    }
}
