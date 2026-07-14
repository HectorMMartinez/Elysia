using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.plato;
using Elysia.Core.Application.Interfaces;
using Elysia.Presentation.WebApi.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class PlatoController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPlatoService platoService;


        public PlatoController(IMapper _mapper, IPlatoService platoService)
        {
            this._mapper = _mapper;
            this.platoService = platoService;
        }


        [HttpGet("get-all-con-ingredientes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MostrarPlatoConIngredientesDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var id = User.FindFirst("UId").Value;

                var data = await platoService.GetlAllConIngredientesAsync(id);
                if (data == null || data.Count == 0)
                {

                    return NotFound("No hay platos registrados");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


        [HttpGet("get-by-id-con-ingrediente/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MostrarPlatoConIngredientesDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdConIgrendiente(int id)
        {
            try
            {
                var data = await platoService.GetByIdConIgrendientesAsync(id);
                if (data == null)
                {

                    return NotFound("No se encontro un plato registrado con ese id...");
                }

                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }




        [HttpPost("add-plato")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatoResponseDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsycn([FromForm] CreatePlatoRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores del plato correctamente");
                }

               
                if (dto.ProductoQuantityDtos == null || dto.ProductoQuantityDtos.Count == 0)
                {
                    return BadRequest("Debes indicar los productos que utilizaras para el plato");
                }


                var id = User.FindFirst("UId")!.Value;
                var map = _mapper.Map<CreatePlatoDto>(dto);
                map.Fecha = DateOnly.FromDateTime(DateTime.Now);
                map.IdPropietario = id;
                map.Imagen = FileHandler.Upload(dto.Imagen, id, "Platos", false);
                map.Codigo = $"PLA-{GetCode()}";
                var data = await platoService.AddAsync(map);


                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }



        [HttpPut("update-Plato/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatoResponseDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] EditarPlatoRequestDto? dto)
        {
            try
            {
                if (dto == null || id <= 0)
                {
                    return BadRequest("Debes ingresar los valores del plato correctamente");
                }

              
                if (dto.ProductoQuantityDtos == null || dto.ProductoQuantityDtos.Count == 0)
                {
                    return BadRequest("Debes indicar los productos que utilizaras para el plato");
                }
                Console.WriteLine(dto.ProductoQuantityDtos);

                var propietarioId = User.FindFirst("UId")!.Value;
                var map = _mapper.Map<EditarPlatoDto>(dto);
                if (dto.Imagen != null)
                {
                    map.Imagen = FileHandler.Upload(dto.Imagen, propietarioId, "Platos", true);
                }

                var data = await platoService.UpdateAsync(id, map);
                return Ok(data);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }





        [HttpDelete("delete-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes especificar el plato a eliminar");
                }


                var plato = await platoService.GetByIdAsync(id);
                var data = await platoService.DeleteAsync(id);
                if (!data || plato == null)
                {
                    return NotFound("No se encontro un plato con ese id, Delete Failed");
                }

                FileHandler.DeleteImage(plato.Imagen);
                return NoContent();

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }






        #region private method
        private string GetCode()
        {
            return $"ZLA-{Random.Shared.Next(100000, 999999)}";
        }
        #endregion



























    }
}
