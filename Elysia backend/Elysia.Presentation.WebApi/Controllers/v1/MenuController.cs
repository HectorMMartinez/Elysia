using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.menu;
using Elysia.Core.Application.Dtos.platoMenu;
using Elysia.Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class MenuController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMenuService menuService;
        private readonly IPlatoMenuService platoMenuService;




        public MenuController(IPlatoMenuService platoMenuService, IMapper mapper, IMenuService menuService)
        {
            this.mapper = mapper;
            this.menuService = menuService;
            this.platoMenuService = platoMenuService;
        }


        [HttpGet("get-all-menu-con-platos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MostrarMenuConPlatosListDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMenusConPlatos()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;

                var data = await platoMenuService.GetMenusConPlatosAsync(user_id);  

                if (data != null && data.HasError)
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






        [HttpPost("add-menu")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMenuAsync([FromBody] CreateMenuRequestDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar correctamente los valores para crear el menu");
                  
                }
                var user_id = User.FindFirst("UId")!.Value;
                var map = mapper.Map<CreateMenuDto>(dto);
                map.FechaCreacion = DateTime.Now;
                map.FechaActualizacion = DateTime.Now;
                map.IdPropietario = user_id;

                var data = await menuService.AddAsync(map);
                return Ok(data);

            }
            catch (Exception ex)
            {
            
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
             
            }
           
        }



        [HttpPut("update-menu/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMenuAsync(int id,[FromBody] EditarMenuRequestDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar correctamente los valores para editar el menu");

                }

                var menu = await menuService.GetByIdAsync(id);

                if (menu == null)
                {
                    return BadRequest("No se encotro un menu con ese id");
                }

                var user_id = User.FindFirst("UId")!.Value;
                var map = mapper.Map<EditarMenuDto>(dto);
                map.FechaCreacion = menu.FechaCreacion;
                map.FechaActualizacion = DateTime.Now;
                map.IdPropietario =menu.IdPropietario;
                map.Id = menu.Id;


                var data = await menuService.UpdateAsync(id,map);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }





        [HttpDelete("delete-menu/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMenuAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente los valores para eliminar el menu");

                }


                var deleted = await menuService.DeleteAsync(id);


                if (!deleted)
                {
                    return NotFound("No se encotro un menu con ese id");
                }

                return Ok("Menu eliminado correctamente");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }




        [HttpGet("get-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMenuAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente los valores para obtener el menu");

                }


                var menu = await menuService.GetByIdAsync(id);


                if (menu == null)
                {
                    return NotFound("No se encotro un menu con ese id");
                }

                return Ok(menu);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }




        [HttpPost("add-platos-all-un-menu")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPlatosAlMenuAsync([FromBody] CreatePlatoMenuDataDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar correctamente los valores para agregar los platos al menu");

                }


                var data = await platoMenuService.AddRangeAsync(dto);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }




        [HttpDelete("delete-plato-de-un-menu/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlatoDelMenuAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                     //indicar el id de la relacion platoMenus
                    return BadRequest("Debes ingresar correctamente el id para eliminar el plato del menu");

                }



                var data = await platoMenuService.DeleteAsync(id);
                if (data)
                {
                    return Ok("Plato eliminado correctamente");
                }

                return NotFound("No se encontro la entidad especificada, no se pudo eliminar el plato del menu");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }





    }
}
