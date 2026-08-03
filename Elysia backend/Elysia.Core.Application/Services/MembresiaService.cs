

using AutoMapper;
using Elysia.Core.Application.Dtos.membresia;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Core.Application.Services
{
    public class MembresiaService : GenericService<Membresia, MembresiaResponseDto, EditMembresiaDto, SaveMembresiaDto>, IMembresiaService
    {

        private readonly IMembresiaRepository repo;
        private readonly IPlanRepository planRepository; 
        private readonly IAccountServices accountServices;
        private readonly IMapper mapper;

        public MembresiaService(IMembresiaRepository genericRepository, IPlanRepository planRepository, IAccountServices accountServices, IMapper _mapper) : base(genericRepository, _mapper)
        {
            this.repo = genericRepository;
            this.planRepository = planRepository;
            this.accountServices = accountServices;
            this.mapper = mapper;
        }


        public async Task<bool> CambiarEstadoAsync(int id, MembresiaEstado estado)
        {
            try
            {


                if (estado != MembresiaEstado.Suspendida || estado != MembresiaEstado.Cancelada || estado != MembresiaEstado.Vencida || estado != MembresiaEstado.Activa)
                {
                    return false;

                }


                return await repo.CambiarEstado(id, estado);


            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar cambiar el estado de la mebresia, favor verificar" + ex.Message);

            }

        }



        public async Task<Membresia?> GetMembresiaByPropietarioId(string id)
        {
            try
            {
                var data = await repo.GetMembresiaByPropietarioId(id);

                return data;


            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener la membresia del propietario especificado" + ex.Message);


            }
        }


        public async Task<List<MostrarMembresiaConPropietarioDto>?> GetAllMembresiaConPropietario()
        {
            try
            {
                var data = await repo.GetlAllAsync();
                var listMembresia = new List<MostrarMembresiaConPropietarioDto>();
                if(data != null || data.Count >= 1)
                {
                    foreach(var item in data)
                    {
                        var plan = await planRepository.GetByIdAsync(item.PlanId);
                        var user = await accountServices.GetUserById(item.UsuarioId);

                        var membresia = new MostrarMembresiaConPropietarioDto() 
                                        { Id = item.Id,
                                          UserName = user.UserName,
                                          NombrePlan = plan.Nombre,
                                          UsuarioId = item.UsuarioId, 
                                          PlanId = item.PlanId, 
                                          NombreRestaurante = user.NombreRestaurante,
                                          FechaFin = item.FechaFin,
                                          FechaInicio = item.FechaInicio,
                                          Estado = item.Estado };

                        listMembresia.Add(membresia);   
                    }


                    foreach (var item in listMembresia)
                    {
                        if (item.FechaInicio >= item.FechaFin)
                        {

                            item.Estado = MembresiaEstado.Suspendida;
                            var map = mapper.Map<Membresia>(item);
                            await repo.UpdateAsync(item.Id, map);
                            listMembresia.Add(item);


                        }

                    }


                    return listMembresia;

                }


                return new List<MostrarMembresiaConPropietarioDto>();




            }catch(Exception ex)
            {



                throw new Exception("Ocurrio un error al intentar obtener las Membresias" + ex.Message);


            }
       
        }

    }
}
