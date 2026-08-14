using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers
{
    [Route("Api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
       
        //controlador base segun la version



    }
}
