using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class MovimientoInventarioRepository : GenericRepository<MovimientoInventario>, IMovimientoRepository
    {
        public MovimientoInventarioRepository(ElysiaContext appContext) : base(appContext)
        {
        }
    }
}
