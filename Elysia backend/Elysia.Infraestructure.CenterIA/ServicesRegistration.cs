using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Settings;
using Elysia.Infraestructure.CenterIA.Configuration;
using Elysia.Infrastructure.AI.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.CenterIA
{
    public static class ServicesRegistration
    {
        
        public static void AddCenterIdLayerIOCForWebApi(this IServiceCollection service, IConfiguration config)
        {

            #region configuration settings
            service.Configure<OpenAISettings>(config.GetSection("OpenAISettings"));
            service.AddHttpClient<IOpenAIProvider, OpenAIProvider>();
            #endregion

            service.AddScoped<IOpenAIProvider, OpenAIProvider>();

        }


    }
}
