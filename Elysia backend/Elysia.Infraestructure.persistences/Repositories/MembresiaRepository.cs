

using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class MembresiaRepository : GenericRepository<Membresia>, IMembresiaRepository
    {
        private readonly ElysiaContext AppContext;
        public MembresiaRepository(ElysiaContext appContext) : base(appContext)
        {

            this.AppContext = appContext;
        }



        public async Task<bool> CambiarEstado(int id, MembresiaEstado estado)
        {
            var mebresia = await AppContext.Set<Membresia>().FindAsync(id);

            if (mebresia != null) 
            {
                mebresia.Estado = estado;   
                await AppContext.SaveChangesAsync();    
                return true;
            }


            return false;
           
        }

        public async Task<Membresia?> GetMembresiaByPropietarioId(string id)
        {
            var membresia = await AppContext.Set<Membresia>().Where(x => x.UsuarioId == id && x.Estado == MembresiaEstado.Activa).FirstOrDefaultAsync();

            if (membresia== null)
            {
                return null;

            }

            return membresia;



        }
    }
}
