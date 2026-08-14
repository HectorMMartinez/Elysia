using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.membresia;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class MembresiaController : BaseApiController
    {

        private readonly IMapper mapper;
        private readonly IMembresiaService service;


        public MembresiaController(IMapper mapper, IMembresiaService service)
        {
            this.mapper = mapper;
            this.service = service;

        }


        [HttpGet("get-all-membresia")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMembresias()
        {
            try
            {

                var data = await service.GetAllMembresiaConPropietario();

                if (data == null)
                {
                    return NotFound("No se encontraron membresias registradas");
                }


                return Ok(data);


            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
             
            
            }
           
        }




        [HttpPut("cancelar-membresia/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelarMembresia(int id)
        {
            try
            {

                var data = await service.GetByIdAsync(id);

                if (data == null)
                {
                    return NotFound("No se encontraron membresias registradas con ese id, favor verificar");
                }

              
                var result = await service.CambiarEstadoAsync(data.Id,MembresiaEstado.Cancelada);

                if(!result)
                {
                    return NotFound("Ocurrio un error al intentar cancelar la membresia");
                }

                return Ok("Membresia cancelada correctamente");

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }




        [HttpPut("suspender-membresia/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SuspenderMembresia(int id)
        {
            try
            {

                var data = await service.GetByIdAsync(id);

                if (data == null)
                {
                    return NotFound("No se encontraron membresias registradas con ese id, favor verificar");
                }

               
                var result = await service.CambiarEstadoAsync(data.Id, MembresiaEstado.Suspendida);

                if (!result)
                {
                    return NotFound("Ocurrio un error al intentar suspender la membresia");
                }

                return Ok("Membrecia Suspendida Correctamente");

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }


        [HttpPut("activar-membresia-por-un-mes/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivarMembresia(int id)
        {
            try
            {

                var data = await service.GetByIdAsync(id);

                if (data == null)
                {
                    return NotFound("No se encontraron membresias registradas con ese id, favor verificar");
                }


                data.Estado = MembresiaEstado.Activa;
                var map = mapper.Map<EditMembresiaDto>(data);
                map.Id = id;
                map.FechaFin = DateTime.Now.AddMonths(1); 
                var result = await service.UpdateAsync(data.Id, map);

                if (result == null)
                {
                    return NotFound("Ocurrio un error al intentar activar la membresia");
                }

                return Ok("Membresia activada correctamente");

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }





    }
}
