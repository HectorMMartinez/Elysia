using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class CategoriaPlatoRepository : GenericRepository<CategoriaPlato>, ICategoriaPlatoRepository
    {
        public CategoriaPlatoRepository(ElysiaContext appContext) : base(appContext)
        {
        }


    }
}
