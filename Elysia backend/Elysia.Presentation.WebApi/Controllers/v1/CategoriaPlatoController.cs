using Asp.Versioning;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class CategoriaPlatoController : BaseApiController
    {
        private readonly ICategoriaPlatoService categoriaPlatoService;


        public CategoriaPlatoController(ICategoriaPlatoService service)
        {
            this.categoriaPlatoService = service;
        }



        [HttpGet("get-all-categoriaPlatos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaPlato))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategoriasPlatos()
        {
            try
            {
                var response = await categoriaPlatoService.GetlAllAsync();

                if (response == null || response.Count == 0)
                {
                    return NotFound("No se encontraron categorias de platos registradas");
                }


                return Ok(response);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);    

            }
        }



    }
}
