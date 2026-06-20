

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


        public MembresiaService(IMembresiaRepository genericRepository, IMapper _mapper) : base(genericRepository, _mapper)
        {
            this.repo = genericRepository;
        }


        public async Task<bool> CambiarEstadoAsync(int id, MembresiaEstado estado)
        {
            try
            {


                if(estado != MembresiaEstado.Suspendida || estado != MembresiaEstado.Cancelada || estado != MembresiaEstado.Vencida || estado != MembresiaEstado.Activa)
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
    }
}
