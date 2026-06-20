
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class GenericRepository<Tentity> : IGenericRepository<Tentity> where Tentity : class
    {

        private readonly ElysiaContext _appContext;

        public GenericRepository(ElysiaContext appContext)
        {

            _appContext = appContext;

        }




        public virtual async Task<Tentity> AddAsync(Tentity entity)
        {

            await _appContext.Set<Tentity>().AddAsync(entity);
            await _appContext.SaveChangesAsync();
            return entity;
        }



        public virtual async Task<bool> DeleteAsync(int id)
        {

            var Entity = await _appContext.Set<Tentity>().FindAsync(id);
            if (Entity != null)
            {
                _appContext.Set<Tentity>().Remove(Entity);
                await _appContext.SaveChangesAsync();
                return true;
            }

            return false;
        }



        public IQueryable<Tentity> GetAllQuariableAsync()
        {
            return _appContext.Set<Tentity>().AsQueryable();
        }



        public virtual async Task<Tentity?> GetByIdAsync(int id)
        {
            var Entity = await _appContext.Set<Tentity>().FindAsync(id);
            if (Entity != null)
            {
                return Entity;
            }

            return null;

        }


        public virtual async Task<List<Tentity>> GetlAllAsync()
        {
            return await _appContext.Set<Tentity>().ToListAsync();
        }



        public virtual async Task<Tentity?> UpdateAsync(int id, Tentity entity)
        {
            var EditEntity = await _appContext.Set<Tentity>().FindAsync(id);
            if (EditEntity != null)
            {

                _appContext.Entry(EditEntity).CurrentValues.SetValues(entity);
                await _appContext.SaveChangesAsync();
                return EditEntity;

            }

            return null;

        }
    }

}

