using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.reservas;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class ReservaController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IReservaServices reservaServices;
        private readonly UserManager<AppUser> userManager;

        public ReservaController(IMapper _mapper, UserManager<AppUser> userManager, IReservaServices reservaServices)
        {
           this._mapper = _mapper;
           this.reservaServices = reservaServices;
           this.userManager = userManager;   
        
        
        }



        [HttpGet("get-all-reservas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<IActionResult> GetAll()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;

                var data = await reservaServices.GetAllReservasByPropietario(user_id);
                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron reservas registradas");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener las reservas del propietario");


            }
        }


        [HttpGet("get-all-reservas-activas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllReservasActivas()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;

                var data = await reservaServices.GetReservasActivasByPropietario(user_id);
                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron reservas  registradas en estado (activas)");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener las reservas (activas) del propietario");


            }
        }



        [HttpGet("get-all-reservas-finalizadas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllReservasFinalizadas()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;

                var data = await reservaServices.GetReservasFinalizadasByPropietario(user_id);
                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron reservas  registradas en estado (finalizas)");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener las reservas (finalizas) del propietario");


            }
        }


        [HttpGet("get-all-reservas-canceladas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllReservasCanceladas()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;

                var data = await reservaServices.GetReservasCanceladaByPropietario(user_id);
                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron reservas  registradas en estado (canceladas)");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener las reservas (canceladas) del propietario");


            }
        }




        [HttpGet("get-all-reservas-no-asistio")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllReservasNoAsistio()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;

                var data = await reservaServices.GetReservasNoAsistioByPropietario(user_id);
                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron reservas  registradas en estado (NoAsistio)");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener las reservas (NoAsistio) del propietario");


            }
        }




        [HttpPost("add-reserva")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReservaAsync([FromBody] CreateReservaRequestDto? dto)
        {
            try
            {
                if(dto == null)
                {
                    return BadRequest("Debes indicar los valores correctamente para agregar la reserva");
                }

                var horaReserva = dto.FechaReserva.Hour;
           
                var user_id = User.FindFirst("UId")!.Value;
                var user = await userManager.FindByIdAsync(user_id);
                var horaCierre = user.HoraCierre.Value.ToTimeSpan();
                if (horaReserva > horaCierre.Hours)
                {
                    return BadRequest("La hora de la reserva no es valida, el restaurante cierra antes");
                }

                var map = _mapper.Map<CreateReservaDto>(dto); 
                map.FechaActualizacion = DateTime.Now;
                map.FechaCreacion = map.FechaActualizacion;   
                map.IdPropietario = user_id;
                var data = await reservaServices.AddAsync(map); 
                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar agregar la reserva");


            }
        }



        [HttpPut("update-reserva/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservaAsync(int id,[FromBody] EditarReservaRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes indicar los valores correctamente para agregar la reserva");
                }

                var horaReserva = dto.FechaReserva.Hour;


                var user_id = User.FindFirst("UId")!.Value;
                var user = await userManager.FindByIdAsync(user_id);
                var horaCierre = user.HoraCierre.Value.ToTimeSpan();
                if (horaReserva > horaCierre.Hours)
                {
                    return BadRequest("La hora de la reserva no es valida, el restaurante cierra antes");
                }

                var map = _mapper.Map<EditarReservaDto>(dto);
                map.FechaActualizacion = DateTime.Now;
                
                var data = await reservaServices.UpdateAsync(id,map);
                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar editar la reserva");


            }
        }




        [HttpGet("get-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReservaByidAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar el id correctamente");
                }

            
                var data = await reservaServices.GetByIdAsync(id);
                if(data == null)
                {
                    return NotFound("No se encontro una reserva con ese id");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener la reserva");


            }
        }


        [HttpDelete("delete-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar el id correctamente");
                }


                var data = await reservaServices.DeleteAsync(id);
                if (data)
                {
                    return Ok("Reserva eliminada correctamente");
                }

                return NotFound("No se encontro una reserva con ese id");

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar eliminar la reserva");


            }
        }

    }
}
