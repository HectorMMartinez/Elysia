using Asp.Versioning;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class DashboardAdminController : BaseApiController
    {
        private readonly IDashboardAdminService dashboardAdminService;

        public DashboardAdminController(IDashboardAdminService dashboardAdminService)
        {
            this.dashboardAdminService = dashboardAdminService;
        }


        [HttpGet("get-panel-admin")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPanel()
        {


            try
            {
                var user_id = User.FindFirst("UId")!.Value;
                var data = await dashboardAdminService.GetPanelAdmin(user_id);
                if (data == null)
                {

                    return NotFound("Ocurrio un error al intentar obtener el panel del admin");
                
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
