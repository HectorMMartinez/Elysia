using Asp.Versioning;
using AutoMapper;
using Elysia.Core.Application.Dtos.pedido;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Elysia.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Propietario")]
    public class PedidoController : BaseApiController
    {
        private readonly IMapper mapper;
        private readonly IPedidoService pedidoService;

        public PedidoController(IMapper mapper, IPedidoService pedidoService)
        {
            this.mapper = mapper;
            this.pedidoService = pedidoService;

        }

        [HttpGet("get-all-pedidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var user_id = User.FindFirst("UId")!.Value;
                var data = await pedidoService.GetPedidoConPlatosByPropietarioId(user_id);

                if (data == null || data.Count == 0)
                {
                    return NotFound("No se encontraron pedido registrado por este propietario");

                }

                return Ok(data);

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }
        }


        

        [HttpPut("cambiar-pedido-en-Preparacion/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PedidoEnPreparacion(int id)
        {
            try
            {

                if(id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente el id, para cambiar el estado del pedido");
                }


                var pedido_exist = await pedidoService.GetByIdAsync(id);
                if (pedido_exist == null)
                {
                    return NotFound("No se encontro un pedido con ese id, favor verificar");

                }
               
                var data = await pedidoService.CambiarEstadoAsync(id,EstadoPedido.EnPreparacion);
                if (!data)
                {
                    return BadRequest("Ocurrio un error al intentar colocar el pedido en preparacion");
                }

                var pedido = await pedidoService.GetByIdAsync(id);
                var map = mapper.Map<EditarPedidoDto>(pedido);
                var updatePedido = await pedidoService.UpdateAsync(pedido.Id,map);

                return Ok("Pedido colocado en preparacion correctamente");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("cambiar-pedido-listo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PedidoListo(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente el id, para cambiar el estado del pedido");
                }


                var pedido_exist = await pedidoService.GetByIdAsync(id);
                if (pedido_exist == null)
                {
                    return NotFound("No se encontro un pedido con ese id, favor verificar");

                }

                var data = await pedidoService.CambiarEstadoAsync(id, EstadoPedido.Listo);
                if (!data)
                {
                    return BadRequest("Ocurrio un error al intentar colocar el pedido como Listo");
                }

                var pedido = await pedidoService.GetByIdAsync(id);
                var map = mapper.Map<EditarPedidoDto>(pedido);
                var updatePedido = await pedidoService.UpdateAsync(pedido.Id, map);

                return Ok("Pedido cambiado a listo correctamente");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPut("cancelar-pedido/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente el id, para cambiar el estado del pedido");
                }


                var pedido_exist = await pedidoService.GetByIdAsync(id);
                if (pedido_exist == null)
                {
                    return NotFound("No se encontro un pedido con ese id, favor verificar");

                }

                var data = await pedidoService.CambiarEstadoAsync(id, EstadoPedido.Cancelado);
                if (!data)
                {
                    return BadRequest("Ocurrio un error al intentar colocar el pedido como Cancelado");
                }


                var pedido = await pedidoService.GetByIdAsync(id);
                var map = mapper.Map<EditarPedidoDto>(pedido);
                var updatePedido = await pedidoService.UpdateAsync(pedido.Id, map);

                return Ok("Pedido cancelado correctamente");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        [HttpPut("finalizar-pedido/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FinalizarPedido(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente el id, para cambiar el estado del pedido");
                }


                var pedido_exist = await pedidoService.GetByIdAsync(id);
                if (pedido_exist == null)
                {
                    return NotFound("No se encontro un pedido con ese id, favor verificar");

                }

                var data = await pedidoService.CambiarEstadoAsync(id, EstadoPedido.Finalizado);
                if (!data)
                {
                    return BadRequest("Ocurrio un error al intentar colocar el pedido como Finalizado");
                }


                var pedido = await pedidoService.GetByIdAsync(id);
                var map = mapper.Map<EditarPedidoDto>(pedido);
                var updatePedido = await pedidoService.UpdateAsync(pedido.Id, map);

                return Ok("Pedido Finalizado correctamente");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }





        [HttpPost("add-pedido")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPedido(CreatePedidoRequestDto? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Debes ingresar correctamente los datos para agregar el pedido");
                }


                var user_id = User.FindFirst("UId")!.Value;
                var map = mapper.Map<CreatePedidoDto>(dto);
                map.Estado = EstadoPedido.Pendiente;
                map.IdPropietario = user_id;
                map.FechaCreacion = DateTime.Now;
                map.FechaActualizacion = map.FechaCreacion;
                var data = await pedidoService.AddAsync(map);

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



        [HttpPut("update-pedido/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePedido(int id, EditarPedidoRequestDto? dto)
        {
            try
            {
                if (dto == null || id <= 0)
                {
                    return BadRequest("Debes ingresar correctamente los datos para editar el pedido");
                }


                var pedidoConDetalles = await pedidoService.GetPedidoConPlatosByPedidoId(id);
                if (pedidoConDetalles != null && pedidoConDetalles.HasError)
                {
                    return NotFound(pedidoConDetalles.Errors.FirstOrDefault());
                }


                if (dto.DetallesPedido.Count <= 1)
                {

                    foreach (var detalle in dto.DetallesPedido)
                    {
                        if (detalle.Cantidad <= 0 && detalle.PlatoId <= 0)
                        {

                            dto.DetallesPedido = pedidoConDetalles.MostrarDetalles
                                      .Select(x => new CreateDetallesPedidoRequestDto()
                                      {
                                          PlatoId = x.IdPlato,
                                          Cantidad = x.CantidaPlato,
                                          Observaciones = x.Observaciones
                                      }).ToList();


                        }
                    }

                }


                if(dto.IdMesa == 0 || dto.IdMesa <= 0)
                {
                    dto.IdMesa = pedidoConDetalles.IdMesa;
                }


                var user_id = User.FindFirst("UId")!.Value;
                var map = mapper.Map<EditarPedidoDto>(dto);
                map.IdPropietario = user_id;
                map.Estado = dto.Estado;
                map.Id = id;
                map.IdMesa = dto.IdMesa;
                var data = await pedidoService.UpdateAsync(map.Id,map);

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





        [HttpDelete("delete-pedido/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePedido(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar correctamente el pedido que quieres eliminar");
                }


                var pedido = await pedidoService.GetByIdAsync(id);
                if (pedido == null)
                {
                    return NotFound("No existe un pedido con ese id, no puedes eliminar");
                }

                var response = await pedidoService.DeleteAsync(pedido.Id);
                 return Ok("Pedido eliminado correctamente");
               
            }
            catch (Exception ex)
            {



                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }
        }



        [HttpGet("get-pedido-con-detalles/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MostrarPedidoConPlatosDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPedidoById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Debes indicar correctamente el pedido que quieres consultar");
                }


                var pedido = await pedidoService.GetPedidoConPlatosByPedidoId(id);
                if (pedido == null)
                {
                    return NotFound("No existe un pedido con ese id, no puedes consultar");
                }

                return Ok(pedido);

            }
            catch (Exception ex)
            {



                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);


            }
        }




    }
}
