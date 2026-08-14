using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.User;
using Elysia.Core.Application.Interfaces;
using Elysia.Presentation.WebApi.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ManagerAccountController : BaseApiController
    {
        private readonly IAccountServices accountServices;
        private readonly IMapper mapper;


        public ManagerAccountController(IAccountServices accountServices, IMapper mapper)
        {
           this.accountServices = accountServices;  
           this.mapper = mapper;    
        }




        [HttpGet("get-all-user-propietario")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPropietario()
        {
            try
            {
                var data = await accountServices.GetAllUserPropietario();
                if (data == null || data.Count == 0)
                {
                    return NotFound("No existen usuario con el rol propietario registrados");
                
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }



        [HttpGet("get-all-user-admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdmin()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value; 

                var data = await accountServices.GetAllUserAdminExceptoAdminId(user_id);
                if (data == null || data.Count == 0)
                {
                    return NotFound("No existen usuario con el rol admin registrados");

                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }


        //este enpoint se utilizara para editar datos de restaurante o tarjeta de un propietario como tal
        [HttpGet("get-user-by-id/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]   
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {

                if(id == null)
                {
                    return BadRequest("Debes indicar el id correctamente para consultar el usuario");
                }




                var data = await accountServices.GetUserById(id);
                if (data == null)
                {
                    return NotFound("No existen usuario registrados con ese id");

                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }



        //el admin puede activar un usuario
        [HttpPost("activar-user-by-id/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivarUser(string id)
        {
            try
            {

                if (id == null)
                {
                    return BadRequest("Debes indicar el id correctamente para  activar el usuario");
                }




                var data = await accountServices.ActivarUser(id);
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



        //el admin puede inactivar un usuario
        [HttpPost("inactivar-user-by-id/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InactivarUser(string id)
        {
            try
            {

                if (id == null)
                {
                    return BadRequest("Debes indicar el id correctamente para inactivar el usuario");
                }


                var data = await accountServices.InhativarUser(id);
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



   
        [HttpPut("edit-datos-restaurante/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarRestaurante(string id, [FromForm] EditarDatosRestaurante? dto)
        {
            try
            {

                if (dto == null || id == null)
                {
                    return BadRequest("Debes indicar los datos correctamente para editar los datos del restaurante");
                }

                var user_exist = await accountServices.GetUserById(id);
                if (user_exist == null)
                {
                    return NotFound("No se encontro un usuario registrado con ese id, favor verificar");
                }


                var map = mapper.Map<SaveUserRequestDto>(dto);

                if(dto.LogoRestaurante != null)
                {
                    map.LogoRestaurante = FileHandler.Upload(dto.LogoRestaurante,user_exist.Id, "logoRestaurante",true);
                }
                else
                {
                    map.LogoRestaurante = user_exist.LogoRestaurante;
                }
                map.Role = user_exist.Role;
                map.Id = user_exist.Id;
                var data = await accountServices.EditUser(map);
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






        //editar perfil usuario
        [HttpPut("edit-perfil")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<IActionResult> EditarPerfil([FromForm] EditPerfilUserDto? perfilDto)
        {
            try
            {

                if (perfilDto == null) 
                {
                    return BadRequest("Debes introducir los datos bien para editar el perfil");
                }

              
                
                var user_id = User.FindFirst("UId")!.Value;
                var map = mapper.Map<SaveUserRequestDto>(perfilDto);
                var user_exist = await accountServices.GetUserById(user_id);  
                if (perfilDto.ProfileImage != null)
                {
                    map.ProfileImage = FileHandler.Upload(perfilDto.ProfileImage,user_id,"profileImages",true);

                }
                else
                {

                    map.ProfileImage = user_exist.ProfileImage;

                }

                map.Id = user_id;
                map.Role = user_exist.Role;
                var data = await accountServices.EditUser(map);
                
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



        //obtener un propietario solo un propietario puede utilizar este enpoint
        [HttpGet("get-perfil-usuario")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPerfilPropietario()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;

                if (user_id == null || string.IsNullOrEmpty(user_id))
                {
                    return BadRequest("Hubo un error al verificar tu cuenta, el id no fue proveido");
                }

               
                var data = await accountServices.GetUserById(user_id);
                if(data == null)
                {
                    return NotFound("No se encontro un usuario con ese id,favor veirificar");
                }


                //en el front-end solo mostrar lo que puede editar
                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }







    }
}
