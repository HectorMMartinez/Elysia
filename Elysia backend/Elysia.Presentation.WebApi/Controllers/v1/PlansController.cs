using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.membresia;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")] 
    public class PlansController : BaseApiController
    {



        private readonly IPlanService planService;
        private readonly IAccountServices accountServices;
        private readonly IMembresiaService membresiaService;
        private readonly IMapper _mapper;



        public PlansController(IPlanService planService, IAccountServices accountServices, IMembresiaService membresiaService, IMapper _mapper)
        {
            this.planService = planService; 
            this._mapper = _mapper;
            this.accountServices = accountServices;
            this.membresiaService = membresiaService;
        }



        [HttpGet("Get-All-Planes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPlanes() 
        {
            try
            {

                var planes = await planService.GetlAllAsync();

                if (planes == null || planes.Count == 0)
                {

                    return NotFound("No se encontraron planes registrados");

                }

                return Ok(planes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        //enviar el id del usuario
        [HttpPut("cambiar-plan-usuario/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CambiarPlan(string id)
        {
            try
            {
                if(id == null)
                {
                    return BadRequest("Debes ingresar correctamente el id del usuario");

                }



                var user_exist = await accountServices.GetUserById(id);

                if (user_exist == null)
                {
                    return NotFound("No se encontro un usuario con ese id, favor veriicar");
                }


                var membrasia = await membresiaService.GetMembresiaByPropietarioId(user_exist.Id);

                if (membrasia == null)
                {
                    return NotFound("El usuario especificado no cuenta con una membresia activa");
                }

                membrasia.PlanId = 2;
                var map = _mapper.Map<EditMembresiaDto>(membrasia);
                var updateMembresia = await membresiaService.UpdateAsync(membrasia.Id,map);


                if (updateMembresia == null)
                {

                    return NotFound("Ocurrio un error, no se pudo editar la membresia del usuario");

                }

                return Ok("Membresia cambiada correctamente (Plan Premium)");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }






























    }
}
