using Elysia.Core.Application.Interfaces;
using Elysia.Core.Application.Services;
using Elysia.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddServicesLayerIOC(this IServiceCollection services)
        {

            #region generalConfiguration
            services.AddAutoMapper(opt => { }, Assembly.GetExecutingAssembly());
            #endregion



            #region services Registration IOC
            services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
            services.AddScoped<IMembresiaService, MembresiaService>();
            services.AddScoped<ITarjetaService, TarjetaService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IProductoService, productoService>();
            services.AddScoped<ICategoriaPlatoService, CategoriaPlatoService>();
            services.AddScoped<IMovimientoInventarioService, MovimientoInventarioService>();
            services.AddScoped<IPlatoService, PlatoService>();
            services.AddScoped<IMesaService, MesaService>();
            services.AddScoped<IMenuService, MenuServices>();
            services.AddScoped<IPlatoMenuService, PlatoMenuService>();
            services.AddScoped<IReservaServices, ReservaServices>();    
            #endregion


        }

    }
}
