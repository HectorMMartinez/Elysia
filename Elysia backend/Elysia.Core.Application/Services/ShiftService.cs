using AutoMapper;
using Elysia.Core.Application.Dtos.empleado;
using Elysia.Core.Application.Dtos.shift;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class ShiftService : GenericService<Shift, ShiftResponseDto, EditarShiftDto, CreateShiftDto>, IShiftService
    {
        private readonly IMapper _mapper;
        private readonly IShiftRepository shiftRepository;
        private readonly IAccountServices accountServices;



        public ShiftService(IShiftRepository shiftRepository, IAccountServices accountServices, IMapper _mapper) : base(shiftRepository, _mapper)
        {
            this.shiftRepository = shiftRepository;
            this._mapper = _mapper;
            this.accountServices = accountServices;

        }




        public override async Task<ShiftResponseDto?> AddAsync(CreateShiftDto? entity)
        {
            var response = new ShiftResponseDto() { Errors = [], HasError = false };
            try
            {
                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar correctamente los datos para crear el turno");
                    return response;

                }

                if (entity.StartTime >= entity.EndTime)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de inicio no puede estar mas adelantada que la hora de  fin");
                    return response;

                }


                var restaurante_Exist = await accountServices.GetUserById(entity.PropietarioId);
                if (restaurante_Exist == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro un restaurante registrado con ese id");
                    return response;
                }


                if (entity.StartTime < restaurante_Exist.HoraApertura)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de inicio del turno no puede ser anterior al hora de apertura del restaurante");
                    return response;
                }


                if(entity.StartTime > restaurante_Exist.HoraCierre)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de inicio del turno no puede ser adelantada a la hora de cierre del restaurante");
                    return response;

                }

                if(entity.EndTime < restaurante_Exist.HoraApertura)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de fin  del turno no puede ser anterior a la hora de apertura del restaurante");
                    return response;
                }



                if (entity.EndTime > restaurante_Exist.HoraCierre)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de fin  del turno no puede ser adelantada a la hora de cierre del restaurante");
                    return response;
                }


                var data = await shiftRepository.GetAllTurnoByPropietarioId(entity.PropietarioId);
                foreach (var item in data)
                {
                    if (item.StartTime == entity.StartTime && item.EndTime == entity.EndTime)
                    {
                        response.HasError = true;
                        response.Errors.Add($"Ya existe un turno con esos horarios {item.StartTime:HH\\:mm}-{item.EndTime:HH\\:mm}");
                        return response;
                    }

                    if (entity.StartTime < item.EndTime && entity.EndTime > item.StartTime)
                    {
                        response.HasError = true;
                        response.Errors.Add(
                            $"El turno interfiere con el horario existente ({item.StartTime:HH\\:mm} - {item.EndTime:HH\\:mm}).");
                        return response;
                    }

                }



                var result = await base.AddAsync(entity);
                return result;

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar agregar el turno" + ex.Message);

            }

        }


        public async Task<List<ShiftResponseDto>?> GetAllTurnoByPropietarioId(string PropietarioId)
        {
            try
            {
                var data = await shiftRepository.GetAllTurnoByPropietarioId(PropietarioId);
                var listShift = new List<ShiftResponseDto>();
                if (data.Any())
                {
                    var map = _mapper.Map<List<ShiftResponseDto>>(data);
                    return map.ToList();    
                }

                return new List<ShiftResponseDto>();

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener los turnos del restaurante actual" + ex.Message);
            
            
            }
        }



        public override async Task<ShiftResponseDto?> UpdateAsync(int id,EditarShiftDto? entity)
        {
            var response = new ShiftResponseDto() { Errors = [], HasError = false };
            try
            {
                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar correctamente los datos para crear el turno");
                    return response;

                }

                var turno_exist = await shiftRepository.GetByIdAsync(id);   
                if(turno_exist == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No existe un turno con ese id, favor verificar");
                    return response;

                }


                if (entity.StartTime >= entity.EndTime)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de inicio no puede estar mas adelantada que la hora de  fin");
                    return response;

                }


                var restaurante_Exist = await accountServices.GetUserById(entity.PropietarioId);
                if (restaurante_Exist == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro un restaurante registrado con ese id");
                    return response;
                }


                if (entity.StartTime < restaurante_Exist.HoraApertura)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de inicio del turno no puede ser anterior al hora de apertura del restaurante");
                    return response;
                }


                if (entity.StartTime > restaurante_Exist.HoraCierre)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de inicio del turno no puede ser adelantada a la hora de cierre del restaurante");
                    return response;

                }

                if (entity.EndTime < restaurante_Exist.HoraApertura)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de fin  del turno no puede ser anterior a la hora de apertura del restaurante");
                    return response;
                }



                if (entity.EndTime > restaurante_Exist.HoraCierre)
                {
                    response.HasError = true;
                    response.Errors.Add("La hora de fin  del turno no puede ser adelantada a la hora de cierre del restaurante");
                    return response;
                }



                var data = await shiftRepository.GetAllTurnoByPropietarioId(entity.PropietarioId);
                foreach (var item in data)
                {
                    if (item.StartTime == entity.StartTime && item.EndTime == entity.EndTime && item.Id != id)
                    {
                        response.HasError = true;
                        response.Errors.Add($"Ya existe un turno con esos horarios {item.StartTime:HH\\:mm}-{item.EndTime:HH\\:mm}");
                        return response;
                    }

                    if (entity.StartTime < item.EndTime && entity.EndTime > item.StartTime && item.Id != id)
                    {
                        response.HasError = true;
                        response.Errors.Add(
                            $"El turno interfiere con el horario existente ({item.StartTime:HH\\:mm} - {item.EndTime:HH\\:mm}).");
                        return response;
                    }


                }

                entity.PropietarioId = restaurante_Exist.Id;
                entity.Id = turno_exist.Id;
                var result = await base.UpdateAsync(entity.Id,entity);
                return result;

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar editar el turno" + ex.Message);

            }

        }


    }
}
