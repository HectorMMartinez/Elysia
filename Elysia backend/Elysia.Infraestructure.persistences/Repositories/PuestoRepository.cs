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
    public class PuestoRepository : GenericRepository<Puesto>, IPuestoRepository
    {
        public PuestoRepository(ElysiaContext appContext) : base(appContext)
        {
        }
    }
}
