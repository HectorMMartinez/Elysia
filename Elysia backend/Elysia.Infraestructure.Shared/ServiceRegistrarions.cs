using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Settings;
using Elysia.Infraestructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Elysia.Infraestructure.Shared
{
    public static class ServiceRegistrarions
    {
        public static void AddEmailServicesIOC(this IServiceCollection service, IConfiguration connfig)
        {


            #region email configuration
            service.Configure<MailSettings>(connfig.GetSection("MailSettings"));
            #endregion



            #region services configurationIOC
            service.AddScoped<IEmailServices, EmailServices>();
            #endregion



        }


    }
}
