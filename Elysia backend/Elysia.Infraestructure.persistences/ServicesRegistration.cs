using Elysia.Core.Domain.interfaces;
using Elysia.Infraestructure.persistences.Contexts;
using Elysia.Infraestructure.persistences.Repositories;
using Elysia.Infraestructure.persistences.seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservaBook.Core.Domain.Interfaces;



namespace Elysia.Infraestructure.persistences
{
    public static class ServicesRegistration
    {
        public static void AddPersistencesLayerIOC(this IServiceCollection Service, IConfiguration config)
        {

            #region context configuration
            if (config.GetValue<bool>("useInMemoryDatabase"))
            {

                Service.AddDbContext<ElysiaContext>
                    (opt => opt.UseInMemoryDatabase("ReservaDbMemory"));
            }
            else
            {

                var connectionStrings = config.GetConnectionString("DefaultConnection");
                Service.AddDbContext<ElysiaContext>(
                   (ServiceProvider, opt) =>
                   {

                       opt.EnableSensitiveDataLogging();
                       opt.UseSqlServer(connectionStrings,
                        m => m.MigrationsAssembly(typeof(ElysiaContext)
                      .Assembly.FullName));

                   },
                    contextLifetime: ServiceLifetime.Scoped,
                    optionsLifetime: ServiceLifetime.Scoped


                );

            }



            #region repositories IOC
            Service.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            Service.AddScoped<IPlanRepository, PlanRepository>();
            Service.AddScoped<ITarjetaRepository, TarjetaRepository>();
            Service.AddScoped<IMembresiaRepository,MembresiaRepository>();  
            #endregion

        }


        public static async Task RunPersistenceSeed(this IServiceProvider Service)
        {

            using var scope = Service.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            var elysiaContext = serviceProvider.GetRequiredService<ElysiaContext>();

            await DefaultPlanes.SeedAsync(elysiaContext);
        }

        #endregion




    }
}

