using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.persistences.Repositories
{
    public class PlatoMenuRepository : GenericRepository<PlatoMenu>, IPlatoMenuRepository
    {


        private readonly ElysiaContext context;

        public PlatoMenuRepository(ElysiaContext appContext) : base(appContext)
        {
            this.context = appContext;
        }


        public async Task<List<PlatoMenu?>> AddRangeAsync(List<PlatoMenu> platoMenus)
        {
            try
            {
                if (platoMenus == null|| platoMenus.Count == 0) 
                {
                    return [];
                
                }


                await context.Set<PlatoMenu>().AddRangeAsync(platoMenus);
                await context.SaveChangesAsync();   
                return platoMenus;


            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar agregar los paltos al menu");
            
            }


        }

        public async Task<List<PlatoMenu?>> GetListByMenuId(int menuId)
        {    
            try
            {
                var data = await context.Set<PlatoMenu>().Where(x => x.IdMenu == menuId).ToListAsync();


                if (data.Any())
                {
                   return data;
                
                }


                return new List<PlatoMenu?> { null };


            }
            catch (Exception ex) 
            {
                throw new Exception("Ocurrio un error al intentar obtener los platos del menu");
           
            }
        }











    }
}
