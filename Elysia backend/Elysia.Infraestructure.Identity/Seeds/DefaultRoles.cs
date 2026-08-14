using Elysia.Core.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.Identity.Seeds
{
    public class DefaultRoles
    {



        public static async Task seedAsync(RoleManager<IdentityRole> roleManager)
        {

            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Propietario.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Empleado.ToString()));

        }


    }
}
