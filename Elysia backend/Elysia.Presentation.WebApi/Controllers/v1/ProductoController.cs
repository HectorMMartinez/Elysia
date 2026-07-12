using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.movimientoInventario;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Application.Interfaces;
using Elysia.Presentation.WebApi.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class ProductoController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductoService service;
        private readonly IMovimientoInventarioService movimientoInventarioService;

        public ProductoController(IMapper _mapper, IProductoService service, IMovimientoInventarioService movimientoInventarioService)
        {
            this._mapper = _mapper;
            this.service = service;
            this.movimientoInventarioService = movimientoInventarioService;
        }


        [HttpGet("get-all-product")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductoResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllProducto()
        {
            try
            {
                var data = await service.GetlAllAsync();

                if (data == null)
                {
                    return BadRequest("No hay categorias de platos registradas");
                }


                return Ok(data);

            }
            catch (Exception ex)
            {

               return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }




        [HttpPost("add-product")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductoResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddProducto([FromForm] SaveProductoDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores del producto correctamente");
                }

                var id = User.FindFirst("UId")!.Value;

                var map = _mapper.Map<CreateProductoDto>(dto);
                map.IdPropietario = id;
                map.Imagen = FileHandler.Upload(dto.Imagen,id,"productos");
                var response = await service.AddAsync(map);
                if(response != null && response.HasError)
                {
                    return BadRequest(response.Errors.FirstOrDefault());
                }


                return Created("Data: Producto agregado correctamente",response);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }



        [HttpPut("edit-product/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductoResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EditarProducto(int id,[FromForm]EditarProductoRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores del producto correctamente");
                }
                var propietarioId = User.FindFirst("UId")!.Value;
                var map = _mapper.Map<EditarProductoDto>(dto);
                if (dto.Imagen != null) {
                    map.Imagen = FileHandler.Upload(dto.Imagen, propietarioId, "productos", true);
                }
                else
                {
                    map.Imagen = "";
                }

                    var response = await service.UpdateAsync(id, map);
                if (response != null && response.HasError)
                {
                    return BadRequest(response.Errors.FirstOrDefault());
                }



                return Ok(response);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }




        [HttpGet("get-product-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductoResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetProductoByid(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar un producto valido, invalid id");
                }



                var response = await service.GetByIdAsync(id);
                if (response == null)
                {
                    return NotFound("Producto no encontrado, no se encontro un producto con ese id");
                }

                return Ok(response);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }


        [HttpDelete("delete-product/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductoResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar un producto valido, invalid id");
                }


                var propietarioId = User.FindFirst("UId")!.Value;
                var producto = await service.GetByIdAsync(id);
                if (producto == null)
                {
                    return  BadRequest("No se pudo encontrar el producto especificado");
                }
                var response = await service.DeleteAsync(id);
                var imageDeleted = FileHandler.DeleteImage(producto.Imagen);
                if (!imageDeleted)
                {
                    return BadRequest("No se pudo eliminar la imagen del producto");
                }

            

                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }

        }





        [HttpPost("add-entrada")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductoResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddEntrada([FromBody] CrearMovimientoInvetarioRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores de la entrada correctamente");
                }


             
                var map = _mapper.Map<CrearMovimientoInventarioDto>(dto);
                map.TipoMovimiento = Core.Domain.Common.TipoMovimientoInventario.Entrada;
                var response = await movimientoInventarioService.AddAsync(map);
                 
                if (response != null && response.HasError)
                {
                    return BadRequest(response.Errors.FirstOrDefault());
                }


                return Created("Data: Movimiento registrado correctamente", response);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPost("add-salida")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductoResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddSalida([FromBody] CrearMovimientoInvetarioRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar los valores de la salida correctamente");
                }


         

                var map = _mapper.Map<CrearMovimientoInventarioDto>(dto);
                map.TipoMovimiento = Core.Domain.Common.TipoMovimientoInventario.Salida;
                var response = await movimientoInventarioService.AddAsync(map);

                if (response != null && response.HasError)
                {
                    return BadRequest(response.Errors.FirstOrDefault());
                }


                return Created("Data: Movimiento registrado correctamente", response);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
