using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.empleado;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class EmpleadoController : BaseApiController
    {
        private readonly IEmpleadoService empleadoService;
        private readonly IMapper mapper;


        public EmpleadoController(IEmpleadoService empleadoService, IMapper mapper)
        {
            this.mapper = mapper;
            this.empleadoService = empleadoService;

        }


        [HttpGet("Get-all-empleados")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEmpleados()
        {
            try
            {

                var user_id = User.FindFirst("UId")!.Value;
                var data = await empleadoService.GetAllEmpleadoConPuesto(user_id);
                if (data.Any())
                {
                    return Ok(data);
                }


                return NotFound("No se encontraron empleados registrados");

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener los empleados" + ex.Message);
            }

        }




        //obtener empleados activos para asociar a un turno
        [HttpGet("Get-all-empleados-activos")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEmpleadosActivos()
        {
            try
            {

                var user_id = User.FindFirst("UId")!.Value;
                var data = await empleadoService.GetAllEmpleadosActivos(user_id);
                if (data.Any())
                {
                    return Ok(data);
                }


                return NotFound("No se encontraron empleados activos registrados registrados");

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener los empleados activos" + ex.Message);
            }

        }



        [HttpGet("Get-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmpleadoById(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest("Debes indicar  un id correcto");
                }

              

                var data = await empleadoService.GetByIdAsync(id);
                if (data == null)
                {
                    return NotFound("No se encontro empleado con ese id, favor verificar");
                }


                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener el empleado" + ex.Message);
            }

        }



        [HttpPost("add-empleado")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddEmpleado(CreateEmpleadoRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes indicar correctamente los datos para agregar un empleado");
                }


                var user_id = User.FindFirst("UId")!.Value;
                var map = mapper.Map<CreateEmpleadoDto>(dto);
                map.RestaurantId = user_id;
                map.HireDate = DateOnly.FromDateTime(DateTime.Now);

                var data = await empleadoService.AddAsync(map);
                if (data == null || data.HasError)
                {
                    return NotFound(data.Errors.FirstOrDefault());
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar agregar el empleado" + ex.Message);
            }

        }


        [HttpPut("edit-empleado/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditEmpleado(int id,EditarEmpleadoRequestDto? dto)
        {
            try
            {
                if (dto == null || id <= 0)
                {
                    return BadRequest("Debes indicar correctamentamente los datos para editar el empleado");
                }



                var map = mapper.Map<EditarEmpleadoDto>(dto);
                map.Id = id;
                var data = await empleadoService.UpdateAsync(map.Id,map);
                if (data == null || data.HasError)
                {
                    return BadRequest(data.Errors.FirstOrDefault());
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar editar el empleado" + ex.Message);
            }

        }

        [HttpDelete("delete-empleado/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar correctamentamente el id para eliminar el empleado");
                }



             
                var data = await empleadoService.DeleteAsync(id);
                if (data)
                {
                    return Ok("Empleado elimindao correctamente");
                }

                return NotFound("Ocurrio un problema al eliminar, no se encontro  un empleado con ese id o no se elimino");

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar eliminar el empleado" + ex.Message);
            }

        }



        [HttpPut("activar-empleado/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivarEmpleado(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar correctamentamente el id para activar el empleado");
                }


                var empleado_exist = await empleadoService.GetByIdAsync(id);
                if (empleado_exist == null)
                {
                    return NotFound("Error: No se encontro un empleado con ese id");
                }

                empleado_exist.IsActive = true;
                var map = mapper.Map<EditarEmpleadoDto>(empleado_exist);
                var data = await empleadoService.UpdateAsync(empleado_exist.Id,map);
                if (data == null || data.HasError)
                {
                    return BadRequest(data.Errors.FirstOrDefault());
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar activar el empleado" + ex.Message);
            }

        }



        [HttpPut("inactivar-empleado/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InactivarEmpleado(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar correctamentamente el id para inactivar el empleado");
                }


                var empleado_exist = await empleadoService.GetByIdAsync(id);
                if (empleado_exist == null)
                {
                    return NotFound("Error: No se encontro un empleado con ese id");
                }

                empleado_exist.IsActive = false;
                var map = mapper.Map<EditarEmpleadoDto>(empleado_exist);
                var data = await empleadoService.UpdateAsync(empleado_exist.Id, map);
                if (data == null || data.HasError)
                {
                    return BadRequest(data.Errors.FirstOrDefault());
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar inactivar el empleado" + ex.Message);
            }

        }









    }
}
