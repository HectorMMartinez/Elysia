using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.Tarjeta;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class ManagerTarjetaController : BaseApiController
    {
        private readonly IMapper mapper;
        private readonly ITarjetaService service;


        public ManagerTarjetaController(IMapper mapper, ITarjetaService service)
        {
            this.mapper = mapper;
            this.service = service;
        }


        //obtener todas las tarjetas
        [HttpGet("get-all-tarjeta")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<IActionResult> GetAllTarjeta()
        {
            try
            {
                var data = await service.GetAllTarjetaConRestaurante();
                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron tarjeta registradas");
              
                }

                return Ok(data);



            }
            catch (Exception ex)
            {
          
                 return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            
            }
        }


        //obtener todas las tarjetas
        [HttpPut("editar-tarjeta/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditarTarjeta(int id,EditarTarjetaRequestDto? dto)
        {
            try
            {

                if(id <= 0 || dto == null)
                {
                    return BadRequest("Debes ingresar los datos correctamente para editar la tarjeta");
                }


                
                var map = mapper.Map<EditTarjetaDto>(dto);
                var data = await service.UpdateAsync(id,map);
                if (data == null || data.HasError)
                {
                    return NotFound(data.Errors.FirstOrDefault());

                }

                return Ok(data);



            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }




        //obtener todas las tarjetas
        [HttpGet("get-tarjeta-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTarjetaById(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Debes ingresar los datos correctamente para editar la tarjeta");
                }



      
                var data = await service.GetByIdAsync(id);
                if (data == null)
                {
                    return NotFound("No se encontro una tarjeta registradas con ese id");

                }

                return Ok(data);



            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }





    }
}
