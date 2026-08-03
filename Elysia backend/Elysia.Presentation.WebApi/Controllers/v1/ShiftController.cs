using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.shift;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{

    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class ShiftController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IShiftService shiftService;


        public ShiftController(IMapper mapper, IShiftService shiftService)
        {
            _mapper = mapper;
            this.shiftService = shiftService;
        }


        [HttpGet("Get-all-turnos-by-restaurante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<IActionResult> GetAllAsync()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;
                var data = await shiftService.GetAllTurnoByPropietarioId(user_id);
                if (data.Any())
                {
                    return Ok(data);
                }

                return NotFound("No se encontraron turno registrados para este restaurante");

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            
            
            }
            
        }



        [HttpGet("Get-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTurnoByIdAsync(int id)
        {
            try
            {
              
                var data = await shiftService.GetByIdAsync(id);
                if (data == null)
                {
                    return NotFound("No se encontro un turno con ese id, favor verificar");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }



        [HttpPost("add-turno")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync(CreateShiftRequestDto? entity)
        {
            try
            {
                if(entity == null || entity.StartTime < TimeOnly.FromDateTime(DateTime.Now) || entity.EndTime < TimeOnly.FromDateTime(DateTime.Now))
                {
                    return BadRequest("Debes indicar correctamente los datos para agregar el turno");
                }

                var user_id = User.FindFirst("UId")!.Value;
                var map = _mapper.Map<CreateShiftDto>(entity);
                map.PropietarioId = user_id;
                var data = await shiftService.AddAsync(map);
                if (data == null || data.HasError)
                {
                    return BadRequest(data.Errors.FirstOrDefault());
                }

                return Ok(data);

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }



        [HttpPut("edit-turno/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id,EditarShiftRequestDto? entity)
        {
            try
            {
                if (id <= 0 || entity == null || entity.StartTime < TimeOnly.FromDateTime(DateTime.Now) || entity.EndTime < TimeOnly.FromDateTime(DateTime.Now))
                {
                    return BadRequest("Debes indicar correctamente los datos para agregar el turno");
                }


                var map = _mapper.Map<EditarShiftDto>(entity);
                var data = await shiftService.UpdateAsync(id,map);
                if (data == null || data.HasError)
                {
                    return BadRequest(data.Errors.FirstOrDefault());
                }

                return Ok(data);

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }


        [HttpDelete("delete-turno/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar correctamente los datos para eliminar el turno");
                }



              
                var data = await shiftService.DeleteAsync(id);
                if (!data)
                {
                    return NotFound("Ocurrio un error al intentar eliminar el turno, puede que no exista un turno con ese id");
                }

                return Ok("Turno eliminado correctamente");

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }












    }
}
