using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Infraestructure.Identity.Entities;
using Elysia.Infraestructure.persistences.Contexts;
using Microsoft.AspNetCore.Identity;


namespace Elysia.Infraestructure.Identity.Seeds
{
    public static class DefaultUserPropietario
    {
       
        public static async Task seedAsync(UserManager<AppUser> userManager,ElysiaContext elysiaContext)
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
                ProfileImage = "https://randomuser.me/api/portraits/men/32.jpg",
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
            var user_exist = await userManager.FindByEmailAsync(email);
            if (user_exist != null)
            {
                await elysiaContext.Membresias.AddAsync(new Membresia()
                {
                    PlanId = 1,
                    UsuarioId = user_exist!.Id,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddMonths(1),
                    Estado = MembresiaEstado.Activa
                });


                await elysiaContext.Tarjetas.AddAsync(new Tarjeta()
                {
                    UsuarioId = user_exist!.Id,
                    NumeroTarjeta = "1234567890123456",
                    AnioVencimiento = 2030,
                    MesVencimiento = 12,
                    FechaRegistro = DateTime.Now,   
                    NombreTitular = "Propietario Dev",
                    Tipo = TipoTarjeta.Visa,
                    CVV = "123"
                });

                await elysiaContext.SaveChangesAsync();
            }

        }

    }

}

