using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class PuestoController : BaseApiController
    {

        private readonly IMapper _mapper;
        private readonly IPuestoService service;




        public PuestoController(IPuestoService service, IMapper mapper)
        {
            _mapper = mapper;
            this.service = service;
        }





        [HttpGet("get-all-puesto")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPuesto()
        {
            try
            {

                var data = await service.GetlAllAsync();
                if (data == null)
                {
                    return NotFound("No se encontraron puesto registrado");
                
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
