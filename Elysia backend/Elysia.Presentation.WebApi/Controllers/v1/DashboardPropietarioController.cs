using Asp.Versioning;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{

    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class DashboardPropietarioController : BaseApiController
    {
        private readonly IDashboardPropietarioServices dashboardPropietarioServices;
        private readonly IAccountServices accountServices;

        public DashboardPropietarioController(IDashboardPropietarioServices dashboardPropietarioServices, IAccountServices accountServices)
        {
           
            this.dashboardPropietarioServices = dashboardPropietarioServices;  
            this.accountServices = accountServices;
        
        }



        [HttpGet("get-panel-propietario")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPanel()
        {
            try
            {

                var user_id = User.FindFirst("UId")!.Value;
                var user_exist = await accountServices.GetUserById(user_id);
                if (user_exist == null)
                {
                    return NotFound("No se encontro un usuario registrado con ese id, favor verificar");
                
                }

                (var data1,var data2) = await dashboardPropietarioServices.GetIndicadoresPanelPropietario(user_id);
                if(data1 == null)
                {
                    return Ok(data2);
                }

                if(data2 == null)
                {
                    return Ok(data1);
                }

                return NotFound("Ocurrio un error al obtener los indicadores del panel del propietario");

            }
            catch (Exception ex)
            {
            
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            
            }
        }
    }
}
