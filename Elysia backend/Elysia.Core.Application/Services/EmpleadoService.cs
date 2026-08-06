using AutoMapper;
using Elysia.Core.Application.Dtos.empleado;
using Elysia.Core.Application.Dtos.movimientoInventario;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class EmpleadoService : GenericService<Empleado, EmpleadoResponseDto, EditarEmpleadoDto, CreateEmpleadoDto>, IEmpleadoService
    {
        private readonly IMapper _mapper;
        private readonly IEmpleadoRepository empleadoRepository;
        private readonly IPuestoRepository puestoRepository;



        public EmpleadoService(IEmpleadoRepository empleadoRepository, IMapper _mapper, IPuestoRepository puestoRepository) : base(empleadoRepository, _mapper)
        {
            this._mapper = _mapper;
            this.empleadoRepository = empleadoRepository;
            this.puestoRepository = puestoRepository;

        }


        public override async Task<EmpleadoResponseDto?> AddAsync(CreateEmpleadoDto? entity)
        {
            var response = new EmpleadoResponseDto() { HasError = false, Errors = [] };

            try
            {
                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar correctamente los datos para agregar el empleado");
                    return response;
               
                }

                var puesto = await puestoRepository.GetByIdAsync(entity.PuestoId);
                if(puesto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("El puesto indicado no esta registrado, no puede agregar un empleado con un puesto inexistente");
                    return response;
                }

                var totalData = await empleadoRepository.GetlAllAsync();
                if(totalData.Any(x => x.Email == entity.Email))
                {
                    response.HasError = true;
                    response.Errors.Add("Ya existe un empleado registrado con ese correo");
                    return response;

                }

                if (totalData.Any(x => x.Phone == entity.Phone))
                {
                    response.HasError = true;
                    response.Errors.Add("Ya existe un empleado registrado con ese numero de telefono");
                    return response;

                }


                if (!entity.Email.Contains("@"))
                {
                    response.HasError = true;   
                    response.Errors.Add($"El correo debe tener un formato valido {entity.Email}");
                    return response;
                }

                if(entity.Phone.Length != 10)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar un numero de telefono valido, 10 digitos sin guiones");
                    return response;
                }


                if(entity.Salary <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar un salario valido para el empleado");
                    return response;
                }

                entity.IsActive = true;
                var data = await base.AddAsync(entity);
                if (data != null)
                { 
                    data.Errors = [];
                    data.HasError = false;
                    return data;
                   
                }

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar agregar el empleado" + ex.Message);
            
            }
        }




        public override async Task<EmpleadoResponseDto?> UpdateAsync(int id,EditarEmpleadoDto? entity)
        {
            var response = new EmpleadoResponseDto() { HasError = false, Errors = [] };

            try
            {
                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar correctamente los datos para agregar el empleado");
                    return response;

                }


                var puesto = await puestoRepository.GetByIdAsync(entity.PuestoId);
                if (puesto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("El puesto indicado no esta registrado, no puede agregar un empleado con un puesto inexistente");
                    return response;
                }

                var empleado = await empleadoRepository.GetByIdAsync(entity.Id);
                if (empleado == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro un empleado con ese id, favor verificar");
                    return response;
                }

                var totalData = await empleadoRepository.GetlAllAsync();
                if (totalData.Any(x => x.Email == entity.Email && x.Id != id))
                {
                    response.HasError = true;
                    response.Errors.Add("Ya existe un empleado registrado con ese correo");
                    return response;

                }

                if (totalData.Any(x => x.Phone == entity.Phone && x.Id != id))
                {
                    response.HasError = true;
                    response.Errors.Add("Ya existe un empleado registrado con ese numero de telefono");
                    return response;

                }


                if (!string.IsNullOrEmpty(entity.Email) && !entity.Email.Contains("@"))
                {
                    response.HasError = true;
                    response.Errors.Add($"El correo debe tener un formato valido {entity.Email}");
                    return response;
                }


                if (!string.IsNullOrEmpty(entity.Phone) &&  entity.Phone.Length != 10)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar un numero de telefono valido, 10 digitos sin guiones");
                    return response;
                }



                if (entity.Salary <= 0)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes indicar un salario valido para el empleado");
                    return response;
                }



                entity.Id = empleado.Id;
                entity.Email = !string.IsNullOrEmpty(entity.Email) ? entity.Email : empleado.Email;
                entity.FirstName = !string.IsNullOrEmpty (entity.FirstName) ? entity.FirstName :    empleado.FirstName;
                entity.LastName = !string.IsNullOrEmpty(entity.LastName) ? entity.LastName : empleado.LastName;
                entity.HireDate = empleado.HireDate;
                entity.Salary = entity.Salary > 0 ? entity.Salary : empleado.Salary;
                entity.PuestoId = entity.PuestoId != 0 ? entity.PuestoId : empleado.PuestoId;
                entity.RestaurantId = empleado.RestaurantId;
                entity.IsActive = empleado.IsActive;    
                entity.Phone = !string.IsNullOrEmpty(entity.Phone) ? entity.Phone : empleado.Phone;
               
                var data = await base.UpdateAsync(entity.Id,entity);
                if (data != null)
                {
                   
                    data.Errors = [];
                    data.HasError = false;
                    return data;

                }

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar editar el empleado" + ex.Message);

            }
        }








        public async Task<List<MostrarEmpleadoConPuestoDto>?> GetAllEmpleadoConPuesto(string restauranteId)
        {
            try
            {
                var listEmpleado = new List<MostrarEmpleadoConPuestoDto>();
                var data = await empleadoRepository.GetAllEmpleadoByPropietarioId(restauranteId);
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        var puesto = await puestoRepository.GetByIdAsync(item.PuestoId);
                        var empleado = new MostrarEmpleadoConPuestoDto()
                        {
                            Id = item.Id,
                            PuestoId = item.PuestoId,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            HireDate = item.HireDate,
                            Email = item.Email,
                            Phone = item.Phone,
                            RestaurantId = item.RestaurantId,
                            IsActive = item.IsActive,
                            Salary = item.Salary,
                            NombrePuesto = puesto.Name
                        };

                        listEmpleado.Add(empleado);


                    }


                    return listEmpleado;
                }



                return new List<MostrarEmpleadoConPuestoDto>();


            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar obtener los empleado con su puesto,para el restaurante especificado" + ex.Message);

            }

        }


















        public async Task<List<EmpleadoResponseDto>> GetAllEmpleadosActivos(string restauranteId)
        {
            try
            {
                var data = await empleadoRepository.GetAllEmpleadosActivos(restauranteId);
                if (data.Any())
                {
                    var map = _mapper.Map<List<EmpleadoResponseDto>>(data);
                    return map.ToList();

                }

                return new List<EmpleadoResponseDto>();
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar los empleados activos" + ex.Message);

            }

        }

        public async Task<List<EmpleadoResponseDto>> GetAllEmpleadosInactivos(string restauranteId)
        {

            try
            {
                var data = await empleadoRepository.GetAllEmpleadosInactivo(restauranteId);
                if (data.Any())
                {
                    var map = _mapper.Map<List<EmpleadoResponseDto>>(data);
                    return map.ToList();

                }

                return new List<EmpleadoResponseDto>();
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar los empleados Inactivos" + ex.Message);

            }

        }
    }
}
