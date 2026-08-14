using Elysia.Core.Application.Dtos.dashboard;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class DashboardAdminService : IDashboardAdminService
    {
        private readonly IAccountServices accountServices;
        private readonly IMembresiaRepository membresiaRepository;
        private readonly IPlanRepository planRepository;
        private readonly ITarjetaRepository tarjetaRepository;


        public DashboardAdminService(IAccountServices accountServices, IMembresiaRepository membresiaRepository, IPlanRepository planRepository, ITarjetaRepository tarjetaRepository)
        {
            this.accountServices = accountServices;
            this.membresiaRepository = membresiaRepository;
            this.planRepository = planRepository;
            this.tarjetaRepository = tarjetaRepository;


        }







        public async Task<MostrarIndicadoresDashboardAdmin> GetPanelAdmin(string adminId)
        {

            try
            {
                var admins = await accountServices.GetAllUserAdmin();
                var plans = await planRepository.GetlAllAsync();
                var propietario = await accountServices.GetAllUserPropietario();
                var tarjeta = await tarjetaRepository.GetlAllAsync();
                var membresias = await membresiaRepository.GetlAllAsync();
                var user = await accountServices.GetUserById(adminId);
                var panel_admin = new MostrarIndicadoresDashboardAdmin()
                {
                    AdminId = adminId,
                    CantidadAdmin = admins.Count,
                    CantidadPlanes = plans.Count,
                    CantidadPropietario = propietario.Count,
                    CantidadTarjetaAmericanExpress = tarjeta.Where(x => x.Tipo == TipoTarjeta.AmericanExpress).Count(),
                    CantidadTarjetaMastercard = tarjeta.Where(x => x.Tipo == TipoTarjeta.Mastercard).Count(),
                    CantidadTarjetaVisa = tarjeta.Where(x => x.Tipo == TipoTarjeta.Visa).Count(),
                    CantidadTotalTarjeta = tarjeta.Where(x => x.Tipo == TipoTarjeta.AmericanExpress).Count() + tarjeta.Where(x => x.Tipo == TipoTarjeta.Mastercard).Count() + tarjeta.Where(x => x.Tipo == TipoTarjeta.Visa).Count(),
                    MembresiasCanceladas = membresias.Where(x => x.Estado == MembresiaEstado.Cancelada).Count(),
                    MembresiasActivas = membresias.Where(x => x.Estado == MembresiaEstado.Activa).Count(),
                    MembresiaSuspendida = membresias.Where(x => x.Estado == MembresiaEstado.Suspendida).Count(),
                    MembresiaVencida = membresias.Where(x => x.Estado == MembresiaEstado.Vencida).Count(),
                    TotalMembresias = membresias.Where(x => x.Estado == MembresiaEstado.Cancelada).Count() + membresias.Where(x => x.Estado == MembresiaEstado.Activa).Count() + membresias.Where(x => x.Estado == MembresiaEstado.Suspendida).Count() + membresias.Where(x => x.Estado == MembresiaEstado.Vencida).Count(),
                    Name = user.UserName ?? "Usuario Desconocido",
                    Image = user.ProfileImage ?? "https://res.cloudinary.com/dxj0gqf4k/image/upload/v1690911685/elysia/elysia-logo-.png"

                };


                return panel_admin;

            }
            catch (Exception ex)
            {



                throw new Exception("Ocurrio un error al intentar obtener el panel de admin" + ex.Message);




            }


        }


    }
}
