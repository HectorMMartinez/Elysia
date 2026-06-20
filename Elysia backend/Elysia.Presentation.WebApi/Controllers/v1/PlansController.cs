using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")] 
    public class PlansController : BaseApiController
    {



        private readonly IPlanService planService;
        private readonly IMapper _mapper;



        public PlansController(IPlanService planService, IMapper _mapper)
        {
            this.planService = planService; 
            this._mapper = _mapper;
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

































    }
}
