using Elysia.Core.Domain.Common;
using Elysia.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.Identity.Seeds
{
    public static class DefaultUserAdmin
    {

        public static async Task seedAsync(UserManager<AppUser> userManager)
        {
            var email = "AdminUser@gmail.com";

            var userExists = await userManager.FindByEmailAsync(email);

            if (userExists != null)
                return;

            var defaultUser = new AppUser
            {
                Name = "Admin",
                LastName = "Dev",
                UserName = "Admin-dev", // sin espacios
                Email = email,
                ProfileImage = "https://res.cloudinary.com/dxjv0gq3e/image/upload/v1690911685/elysia/propietario",
                IsActive = true,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(defaultUser, "123Skey@");

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(defaultUser, UserRoles.Admin.ToString());
        }





    }
}
