using Asp.Versioning;
using Elysia.Core.Application.Dtos.CenterIA;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class CenterIAController : BaseApiController
    {
        private readonly ICenterIAService centerIAService;



        public CenterIAController(ICenterIAService centerIAService)
        {
            this.centerIAService = centerIAService;
        }




        [HttpGet("get-analisis-restaurante")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAnalisisRestauranteAsync()
        {
            try
            {

                var user_id = User.FindFirst("UId")!.Value;
                var data = await centerIAService.AnalizarRestauranteAsync(user_id);
                return Ok(data);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }





        [HttpPost("consultar-asistente")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConsultarAsistenteAsync(SolicitudChatIaDto dto)
        {
            try
            {

                var user_id = User.FindFirst("UId")!.Value;
                var data = await centerIAService.ConsultarAsistenteAsync(user_id, dto);
                return Ok(data);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }





        [HttpGet("get-optimizacion-menu")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OptimizarMenuAsync()
        {
            try
            {

                var user_id = User.FindFirst("UId")!.Value;
                var data = await centerIAService.OptimizarMenuAsync(user_id);
                return Ok(data);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }




    }
}
