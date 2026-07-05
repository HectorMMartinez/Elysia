using Elysia.Core.Domain.Common;
using Elysia.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Infraestructure.Identity.Seeds
{
    public static class DefaultUserPropietario
    {
        public static async Task seedAsync(UserManager<AppUser> userManager)
        {
            var email = "propietarioUser@gmail.com";

            var userExists = await userManager.FindByEmailAsync(email);

            if (userExists != null)
                return;

            var defaultUser = new AppUser
            {
                Name = "Propietario",
                LastName = "Dev",
                UserName = "propietariouser", // sin espacios
                Email = email,
                ProfileImage = "https://res.cloudinary.com/dxjv0gq3e/image/upload/v1690911685/elysia/propietario",
                NombreRestaurante = "Restaurante TEO",
                LogoRestaurante = "https://res.cloudinary.com/dxjv0gq3e/image/upload/v1690911685/elysia/propietario",
                IsActive = true,
                IdCard = "123456789",
                RNC = "123456789",
                HoraApertura = new TimeOnly(8, 0),
                HoraCierre = new TimeOnly(22, 0),
                DireccionRestaurante = "Calle Principal #123",
                PhoneRestaurante = "8095551234",
                Especialidad = "Comida Dominicana",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(defaultUser, "123Pass@");

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(defaultUser, UserRoles.Propietario.ToString());
        }



    }

}

