using AutoMapper;
using Elysia.Core.Application.Dtos.shift;
using Elysia.Core.Application.Dtos.ShiftEmpleado;
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
    public class ShiftEmpleadoService : GenericService<EmployeeShift, ShiftEmpleadoResponseDto, CreateShiftEmpleadoDto, CreateShiftEmpleadoDto>, IShiftEmpleadoService
    {
        private readonly IMapper _mapper;
        private readonly IShiftEmpleadoRepository repo;
        private readonly IShiftRepository shiftRepository;
        private readonly IEmpleadoRepository empleadoRepository;

        public ShiftEmpleadoService(IShiftEmpleadoRepository repo, IShiftRepository shiftRepository, IEmpleadoRepository empleadoRepository, IMapper _mapper) : base(repo, _mapper)
        {
            this.repo = repo;
            this._mapper = _mapper;
            this.shiftRepository = shiftRepository;
            this.empleadoRepository = empleadoRepository;
        }




        public override async Task<ShiftEmpleadoResponseDto?> AddAsync(CreateShiftEmpleadoDto? entity)
        {
            var response  = new ShiftEmpleadoResponseDto() { HasError = false, Errors = []};
            try
            {
                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar los datos correctamente para agregar el turno al empleado");
                    return response;
                }


                var empleado_exist = await empleadoRepository.GetByIdAsync(entity.EmpleadoId);
                if(empleado_exist == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No existe un empleado con ese id, favor verificar");
                    return response;

                }

                var turno_exist = await shiftRepository.GetByIdAsync(entity.ShiftId);
                if(turno_exist == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No existe un turno con ese id, favor verificar");
                    return response;

                }

                if(entity.WorkDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    response.HasError = true;
                    response.Errors.Add("La fecha para trabajar no puede ser anteriol a la fecha actual");
                    return response;

                }

                var data = await base.AddAsync(entity);
                return data;


            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al intentar agregar el turno al empleado" + ex.Message);
            }

        }





        public async Task<List<MostrarShiftEmpleadoConNombreDto>?> MostrarShiftEmpleadoConNombreDtos(string propietarioId)
        {
            try
            {
                var data = await empleadoRepository.GetAllEmpleadoByPropietarioId(propietarioId);
                var listData = new List<MostrarShiftEmpleadoConNombreDto>();
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        var shiftEmpleados = await repo.GetEmployeeShiftsByEmpleadoId(item.Id);
                        if (shiftEmpleados == null || shiftEmpleados.Count == 0)
                        {
                            continue;
                        }

                        if (shiftEmpleados.Count > 0)
                        {
                            foreach (var turno in shiftEmpleados)
                            {
                                var turno_exist = await shiftRepository.GetByIdAsync(turno.ShiftId);

                                listData.Add(new MostrarShiftEmpleadoConNombreDto()
                                {
                                    Id = turno.Id,
                                    EmpleadoId = turno.EmpleadoId,
                                    ShiftId = turno.ShiftId,
                                    NombreEmpleado = item.FirstName,
                                    NombreShift = turno_exist!.Name,
                                    WorkDate = turno.WorkDate
                                });

                            }
                        }

                    }


                    return listData;
                }


                return listData;


            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener los turnos de este propietario" + ex.Message);


            }
        }
    }
}
