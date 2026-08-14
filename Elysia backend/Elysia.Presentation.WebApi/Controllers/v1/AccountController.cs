using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.membresia;
using Elysia.Core.Application.Dtos.Tarjeta;
using Elysia.Core.Application.Dtos.User;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Infraestructure.Identity.Services;
using Elysia.Presentation.WebApi.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {

        private readonly IAccountServices accountServices;
        private readonly IMembresiaService membresiaService;
        private readonly ITarjetaService tarjetaService;

        private readonly IMapper _mapper;



        public AccountController(IAccountServices accountServices, IMembresiaService membresiaService, ITarjetaService tarjetaService, IMapper _mapper)
        {
            this.accountServices = accountServices;
            this.membresiaService = membresiaService;
            this.tarjetaService = tarjetaService;
            this._mapper = _mapper;
        }



        [HttpPost("login-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores correctamente");
                }

                var login = await accountServices.Authenticate(dto);


                if (login != null && login.HasError && login.Errors!.Count > 0)
                {
                    return BadRequest(login.Errors.FirstOrDefault());
                }

                return Ok(login);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

        }






        [HttpPost("register-user")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] RegisterUserRequestDto dto)
        {

            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores correctamente");
                }


                var map = _mapper.Map<SaveUserRequestDto>(dto);
                map.Role = UserRoles.Propietario.ToString();
                var user = await accountServices.RegisterUser(map);

                if (user != null && user.HasError)
                {
                    return BadRequest(user.Errors!.FirstOrDefault());

                }

                map.ProfileImage = FileHandler.Upload(dto.ProfileImage, user!.Id, "profileImages", false, "");
                map.LogoRestaurante = FileHandler.Upload(dto.LogoRestaurante, user.Id, "logoRestaurante");
                map.Id = user.Id;   
                var editUser = await accountServices.EditUser(map,true);
                if (editUser != null && editUser.HasError)
                {
                    return BadRequest(editUser.Errors!.FirstOrDefault());
                }

                await membresiaService.AddAsync(new SaveMembresiaDto() { UsuarioId = user!.Id, PlanId = dto.PlanId, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1), Estado = MembresiaEstado.Activa });
                await tarjetaService.AddAsync(new SaveTarjetaDto() { UsuarioId = user.Id, Tipo = dto.Tipo, AnioVencimiento = dto.AnioVencimiento, MesVencimiento = dto.MesVencimiento, CVV = dto.CVV, NombreTitular = dto.NombreTitular, NumeroTarjeta = dto.NumeroTarjeta, FechaRegistro = DateTime.Now });
                return Created("Informacion", user);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

        }





        [HttpPost("confirm-account")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Confirm([FromBody] ConfirmRequestDto dto)
        {

            try
            {

                var result = await accountServices.confirmAccounAsync(dto);


                if (result != null && result.HasError)
                {
                    return BadRequest(result.Message);
                }
                

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }





        [HttpPost("get-resset-token")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRessetToken([FromBody] ForgotPasswordRequestDto dto)
        {

            try
            {

                var result = await accountServices.ForgotPasswordAsync(dto);


                if (result != null && result.HasError && result.Errors.Count > 0)
                {
                    return BadRequest(result.Errors.FirstOrDefault());
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }



        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] RessetPasswordRequestDto dto)
        {

            try
            {

                var result = await accountServices.RessetPassowrd(dto);


                if (result != null && result!.HasError && result.Errors!.Count > 0)
                {
                    return BadRequest(result.Errors.FirstOrDefault());
                }


                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


    }



}
