using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.Mesa;
using Elysia.Core.Application.Interfaces;
using Elysia.Presentation.WebApi.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class MesaController : BaseApiController
    {
        private readonly IMesaService service;
        private readonly IMapper _Mapper;


        public MesaController(IMesaService service, IMapper mapper)
        {
            this.service = service;
            this._Mapper = mapper;
        }




        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var id = User.FindFirst("UId")!.Value;
                var data = await service.GetAllByPropietarioIdAsync(id);

                if(data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron mesas registradas");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            
            
            }
        }



        [HttpGet("get-all-disponibles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDisponibles()
        {
            try
            {
                var id = User.FindFirst("UId")!.Value;
                var data = await service.GetAllDisponibleByPropietarioId(id);

                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron mesas registradas con estados disponible");
                }

                return Ok(data);
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
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var data = await service.GetByIdAsync(id);

                if (data == null)
                {
                    return NotFound("No se encontro una mesa registrada con ese id");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }
        }



        [HttpPost("add-mesa")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MesaResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromForm] CreateMesaRequestDto? dto)
        {
            try
            {
                if(dto == null)
                {
                    return BadRequest("Debes ingresar los valores de la mesa correctamente");
                }


                var id = User.FindFirst("UId")!.Value;



                var map = _Mapper.Map<CreateMesaDto>(dto);
                map.FechaCreacion = DateTime.Now;
                map.FechaActualizacion = DateTime.Now;
                map.Codigo = $"ME-{GetCode()}";
                map.IdPropietario = id;
                map.Imagen = FileHandler.Upload(dto.Imagen, id, "Mesas", false);
                          
                var data = await service.AddAsync(map);

                if (data == null)
                {
                    return BadRequest("No se pudo agregar la mesa");
                }


                return Ok(data);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }
        }





        [HttpPut("update-mesa/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MesaResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(int id,[FromForm] EditarMesaRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores de la mesa correctamente");
                }


                var PropietarioId = User.FindFirst("UId")!.Value;

                var map = _Mapper.Map<EditarMesaDto>(dto);
                map.FechaActualizacion = DateTime.Now;
                var mesa = await service.GetByIdAsync(id);
                if (mesa == null)
                {
                    return BadRequest("No se encontro una mesa con ese id");
                }
                if (dto.Imagen != null)
                {
                    map.Imagen = FileHandler.Upload(dto.Imagen, PropietarioId, "Mesas", true);
                }
                else
                {
                    map.Imagen = mesa.Imagen;
                }



                    var data = await service.UpdateAsync(id, map);

                if (data != null && data.HasError)
                {
                    return BadRequest(data.Errors.FirstOrDefault());
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }
        }



        [HttpDelete("delete-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente el id, invalid id");
                }



                var mesa = await service.GetByIdAsync(id);
                if (mesa == null)
                {
                   return NotFound("No se encontro una mesa con ese id");
                
                }


                var data = await service.DeleteAsync(id);
                FileHandler.DeleteImage(mesa.Imagen);
                return Ok("Mesa eliminada correctamente");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }
        }




        #region private method
        private string GetCode()
        {
            return $"LAL-{Random.Shared.Next(100000, 999999)}";
        }
        #endregion





    }
}
