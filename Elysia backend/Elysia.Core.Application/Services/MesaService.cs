

using AutoMapper;
using Elysia.Core.Application.Dtos.Mesa;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Core.Application.Services
{
    public class MesaService : GenericService<Mesa, MesaResponseDto, EditarMesaDto, CreateMesaDto>, IMesaService
    {
        private readonly IMapper _mapper;
        private readonly IMesaRepository mesaRepository;
        public MesaService(IMesaRepository mesaRepository, IMapper _mapper) : base(mesaRepository, _mapper)
        {
            this._mapper = _mapper;
            this.mesaRepository = mesaRepository;
        }


        public  async Task<List<Mesa?>> GetAllByPropietarioIdAsync(string propietarioId)
        {
            try
            {

                var data = await mesaRepository.GetAllByPropietarioId(propietarioId);

                if (data.Any())
                {
                    return data;
                }

                return [];

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al intentar obtener las mesas:{ex.Message}");
            
            }


        }




        public async  Task<List<Mesa?>> GetAllDisponibleByPropietarioId(string propietarioId)
        {

            try
            {

                var data = await mesaRepository.GetAllDisponibleByPropietarioId(propietarioId);

                if (data.Any())
                {
                    return data;
                }

                return [];

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al intentar obtener las mesas disponibles:{ex.Message}");

            }

        }











        public override async Task<MesaResponseDto?> UpdateAsync(int id, EditarMesaDto? dto)
        {
             var response  = new MesaResponseDto() { HasError = false, Errors = [] };
            try
            {
                if (dto == null || id <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes ingresar los valores correctamente para editar, verifica los datos o el Id");
                    return response;

                }

                var mesa = await mesaRepository.GetByIdAsync(id);
                if(mesa == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro la mesa especificada para poder editar..");
                    return response;
                }


                var data = _mapper.Map<Mesa>(dto);
                data.Codigo = mesa.Codigo;
                data.FechaCreacion = mesa.FechaCreacion;
                data.IdPropietario = mesa.IdPropietario;
                data.Id = mesa.Id;
                data.Imagen = !string.IsNullOrEmpty(dto.Imagen) ? dto.Imagen : data.Imagen;
                var map = await mesaRepository.UpdateAsync(id, data);
                var dataResponse = _mapper.Map<MesaResponseDto>(map);
                return dataResponse;



            }catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al intentar editar la mesa:{ex.Message}");
            
            }
        }

    }
}
