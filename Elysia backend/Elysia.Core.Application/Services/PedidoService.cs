using AutoMapper;
using Elysia.Core.Application.Dtos.pedido;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Elysia.Core.Application.Services
{
    public class PedidoService : GenericService<Pedido, MostrarPedidoConPlatosDto, EditarPedidoDto, CreatePedidoDto>, IPedidoService
    {
        private readonly IMapper _mapper;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IDetallesPedidoRepository detallesPedidoRepository;
        private readonly IPlatoRepository platoRepository;
        private readonly IMesaRepository mesaRepository;
        private readonly IReservasRepository reservasRepository;
        private readonly IProductoRepository productoRepository;
        private readonly IPlatoProductoRepository platoProductoRepository;



        public PedidoService(IPedidoRepository pedidoRepository, IProductoRepository productoRepository, IPlatoProductoRepository platoProductoRepository, IReservasRepository reservasRepository, IMesaRepository mesaRepository, IPlatoRepository platoRepository, IDetallesPedidoRepository detallesPedidoRepository, IMapper _mapper) : base(pedidoRepository, _mapper)
        {
            this.pedidoRepository = pedidoRepository;
            this.detallesPedidoRepository = detallesPedidoRepository;
            this.platoRepository = platoRepository;
            this.mesaRepository = mesaRepository;
            this.reservasRepository = reservasRepository;
            this._mapper = _mapper;
            this.platoRepository = platoRepository;
            this.platoProductoRepository = platoProductoRepository;
            this.productoRepository = productoRepository;
        }




        public override async Task<MostrarPedidoConPlatosDto?> AddAsync(CreatePedidoDto? dto)
        {
            var response = new MostrarPedidoConPlatosDto() { HasError = false, Errors = [] };

            try
            {
                var fechaActual = DateTime.Now;

                if (dto == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes ingresar los valores correctamente para agregar el pedido");
                    return response;
                }


                var mesa = await mesaRepository.GetByIdAsync(dto.IdMesa);

                if (mesa == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro una mesa con ese id");
                    return response;
                }

                if (mesa.Estado == MesaEstado.Ocupada)
                {
                    response.HasError = true;
                    response.Errors.Add("La mesa se encuentra ocupa, no puede realizar el pedido para esa mesa");
                    return response;
                }


                var reservas = await reservasRepository.GetAllReservasByMesaId(dto.IdMesa);
                if (reservas.Count > 0 || reservas.Any())
                {
                    foreach (var reserva in reservas)
                    {
                        if (reserva.Estado == EstadoReserva.Activa || reserva.Estado == EstadoReserva.EnProceso)
                        {
                            if (reserva.FechaReserva == fechaActual || reserva.FechaReserva.AddHours(4) == fechaActual)
                            {
                                response.HasError = true;
                                response.Errors.Add("No puedes realizar el pedido para esa fecha o hora, la mesa esta reservada");
                                return response;
                            }
                        }


                    }
                }


                var pedidosMesa = await pedidoRepository.GetAllPedidosByMesaId(dto.IdMesa);

                if (pedidosMesa.Count > 0 || pedidosMesa.Any())
                {
                    foreach (var pedidos in pedidosMesa)
                    {
                        if (pedidos.FechaCreacion == fechaActual || pedidos.FechaCreacion.AddHours(4) == fechaActual)
                        {
                            response.HasError = true;
                            response.Errors.Add("No se puede realizar el pedido para esa fecha o hora, la mesa ya tiene un pedido registrado que concide con la fecha");
                            return response;
                        }
                    }

                }

                /*El usuario tendra que indicar el o los platos que querra para su pedido y la cantidad de plato que va a querer por cada tipo de plato,
                 se debe validar que por cada plato y segun los productos que se necesiten para ese plato, halla stock suficiente para sastifacer 
                 esa necesidad*/

                var responseValidateStock = await ValidarYActualizarStockAsync(dto);
                if (responseValidateStock != null && responseValidateStock.HasError.Value)
                {
                    response.HasError = true;
                    response.Errors.Add(responseValidateStock.Errors.FirstOrDefault());
                    return response;

                }


                var dataPedido = await pedidoRepository.AddAsync(new Pedido() { IdMesa = dto.IdMesa, IdPropietario = dto.IdPropietario, Estado = EstadoPedido.Pendiente, FechaActualizacion = dto.FechaActualizacion, FechaCreacion = dto.FechaCreacion });
                var listDetalles = new List<DetallesPedido>();
                decimal total = 0.0m;
                foreach (var detalle in dto.DetallesPedidoDtos)
                {
                    var plato = await platoRepository.GetByIdAsync(detalle.PlatoId);
                    var detallePedido = new DetallesPedido() { PlatoId = detalle.PlatoId, PrecioUnitario = plato.Precio, Cantidad = detalle.Cantidad, PedidoId = dataPedido.Id, Observaciones = detalle.Observaciones };
                    total += detalle.Cantidad * plato.Precio;
                    listDetalles.Add(detallePedido);
                }
                mesa.Estado = MesaEstado.Reservada;
                await mesaRepository.UpdateAsync(mesa.Id, mesa);
                dataPedido.Total = total;
                await pedidoRepository.UpdateAsync(dataPedido.Id, dataPedido);
                var dataDetalles = await detallesPedidoRepository.AddRangeAsync(listDetalles);

                var map = new MostrarPedidoConPlatosDto()
                {
                    Id = dataPedido.Id,
                    IdMesa = dto.IdMesa,
                    NombreMesa = mesa.Nombre,
                    TotalPedido = total,
                    Estado = dataPedido.Estado,
                    MostrarDetalles = dataDetalles
                                     .Select(x => new MostrarDetallesPedidoDto()
                                     {
                                         IdPlato = x.PlatoId,
                                         CantidaPlato = x.Cantidad,
                                         NombrePlato = x.Plato.Nombre,
                                         PrecioPlato = x.Plato.Precio,
                                         Observaciones = x.Observaciones,
                                     })
                                     .ToList(),

                };



                return map;

            }
            catch (Exception ex)
            {


                throw new Exception($"Ocurrio un error  al intentar agregar el pedido: {ex.Message}");


            }
        }





        public async Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosCanceladosAsync(string propietarioId)
        {

            try
            {
                var pedidos = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Cancelado)
                    .ToListAsync();

                return pedidos.Select(pedido => new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(detalle => new MostrarDetallesPedidoDto
                        {
                            IdPlato = detalle.PlatoId,
                            NombrePlato = detalle.Plato.Nombre,
                            PrecioPlato = detalle.PrecioUnitario,
                            CantidaPlato = detalle.Cantidad,
                            Observaciones = detalle.Observaciones,
                        })
                        .ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener los pedidos con estado (cancelados). " + ex.Message);
            }








        }

        public async Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosEnProcesoAsync(string propietarioId)
        {

            try
            {
                var pedidos = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.EnPreparacion)
                    .ToListAsync();

                return pedidos.Select(pedido => new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(detalle => new MostrarDetallesPedidoDto
                        {
                            IdPlato = detalle.PlatoId,
                            NombrePlato = detalle.Plato.Nombre,
                            PrecioPlato = detalle.PrecioUnitario,
                            CantidaPlato = detalle.Cantidad,
                            Observaciones = detalle.Observaciones,
                        })
                        .ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener los pedidos con estado (EnPreparacion). " + ex.Message);
            }

        }


        public async Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosEntregadoAsync(string propietarioId)
        {
            try
            {
                var pedidos = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Entregado)
                    .ToListAsync();

                return pedidos.Select(pedido => new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(detalle => new MostrarDetallesPedidoDto
                        {
                            IdPlato = detalle.PlatoId,
                            NombrePlato = detalle.Plato.Nombre,
                            PrecioPlato = detalle.PrecioUnitario,
                            CantidaPlato = detalle.Cantidad,
                            Observaciones = detalle.Observaciones,
                        })
                        .ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener los pedidos con estado (Entregados). " + ex.Message);
            }
        }



        public async Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosListoAsync(string propietarioId)
        {
            try
            {
                var pedidos = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Listo)
                    .ToListAsync();

                return pedidos.Select(pedido => new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(detalle => new MostrarDetallesPedidoDto
                        {
                            IdPlato = detalle.PlatoId,
                            NombrePlato = detalle.Plato.Nombre,
                            PrecioPlato = detalle.PrecioUnitario,
                            CantidaPlato = detalle.Cantidad,
                            Observaciones = detalle.Observaciones,
                        })
                        .ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener los pedidos con estado (Listo). " + ex.Message);
            }

        }



        public async Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosPendienteAsync(string propietarioId)
        {
            try
            {
                var pedidos = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Pendiente)
                    .ToListAsync();

                return pedidos.Select(pedido => new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(detalle => new MostrarDetallesPedidoDto
                        {
                            IdPlato = detalle.PlatoId,
                            NombrePlato = detalle.Plato.Nombre,
                            PrecioPlato = detalle.PrecioUnitario,
                            CantidaPlato = detalle.Cantidad,
                            Observaciones = detalle.Observaciones,
                        })
                        .ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener los pedidos con estado (Pendiente). " + ex.Message);
            }
        }




        public  async Task<List<MostrarPedidoConPlatosDto>> GetAllPedidosFinalizado(string propietarioId)
        {
            try
            {
                var pedidos = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .Where(x => x.IdPropietario == propietarioId && x.Estado == EstadoPedido.Finalizado)
                    .ToListAsync();

                return pedidos.Select(pedido => new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(detalle => new MostrarDetallesPedidoDto
                        {
                            IdPlato = detalle.PlatoId,
                            NombrePlato = detalle.Plato.Nombre,
                            PrecioPlato = detalle.PrecioUnitario,
                            CantidaPlato = detalle.Cantidad,
                            Observaciones = detalle.Observaciones,

                        })
                        .ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener los pedidos con estado (Finalizado). " + ex.Message);
            }

        }





        public override async Task<MostrarPedidoConPlatosDto?> UpdateAsync(int id, EditarPedidoDto? entity)
        {
            var response = new MostrarPedidoConPlatosDto() { HasError = false, Errors = [] };

            try
            {
                var fechaActual = DateTime.Now;

                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Debes ingresar correctamente los valores del pedido para editar");
                    return response;
                }


                var pedido = await pedidoRepository.GetByIdAsync(id);
                if (pedido == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro un pedido con ese id, no puede editar");
                    return response;
                }

                var detallesPedidio = await detallesPedidoRepository.GetAllByPedidoId(id);



                var mesa = await mesaRepository.GetByIdAsync(entity.IdMesa);

                if (mesa == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro una mesa con ese id");
                    return response;
                }


                if (mesa.Estado == MesaEstado.Ocupada && pedido.Estado != EstadoPedido.EnPreparacion || pedido.Estado != EstadoPedido.Entregado || pedido.Estado != EstadoPedido.Listo)
                {
                    response.HasError = true;
                    response.Errors.Add("La mesa se encuentra ocupa, no puede realizar el pedido para esa mesa");
                    return response;
                }



                var reservas = await reservasRepository.GetAllReservasByMesaId(entity.IdMesa);
                if (reservas.Count > 0 || reservas.Any())
                {
                    foreach (var reserva in reservas)
                    {
                        if (reserva.Estado == EstadoReserva.Activa || reserva.Estado == EstadoReserva.EnProceso)
                        {
                            if (reserva.FechaReserva == fechaActual || reserva.FechaReserva.AddHours(4) == fechaActual)
                            {
                                response.HasError = true;
                                response.Errors.Add("No puedes realizar el pedido para esa fecha o hora, la mesa esta reservada");
                                return response;
                            }
                        }


                    }
                }


                var pedidosMesa = await pedidoRepository.GetAllPedidosByMesaId(entity.IdMesa);

                if (pedidosMesa.Count > 0 || pedidosMesa.Any())
                {
                    foreach (var pedidos in pedidosMesa)
                    {
                        if (pedidos.FechaCreacion == fechaActual || pedidos.FechaCreacion.AddHours(4) == fechaActual)
                        {
                            response.HasError = true;
                            response.Errors.Add("No se puede realizar el pedido para esa fecha o hora, la mesa ya tiene un pedido registrado que concide con la fecha");
                            return response;
                        }
                    }

                }


                /*El usuario tendra que indicar el o los platos que querra para su pedido y la cantidad de plato que va a querer por cada tipo de plato,
                 se debe validar que por cada plato y segun los productos que se necesiten para ese plato, halla stock suficiente para sastifacer 
                 esa necesidad*/



                var responseValidateStock = await ValidarYActualizarStockUpdateAsync(entity, detallesPedidio);

                if (responseValidateStock.HasError == true)
                {
                    response.HasError = true;
                    response.Errors.Add(responseValidateStock.Errors.First());
                    return response;
                }


                if(entity.Estado == EstadoPedido.EnPreparacion)
                {
                    mesa.Estado = MesaEstado.Ocupada;
                    await mesaRepository.UpdateAsync(mesa.Id,mesa);
                }

                if(entity.Estado == EstadoPedido.Listo)
                {
                    mesa.Estado = MesaEstado.Ocupada;
                    await mesaRepository.UpdateAsync(mesa.Id,mesa);
                }

                if(entity.Estado == EstadoPedido.Entregado)
                {
                    mesa.Estado = MesaEstado.Ocupada;
                    await mesaRepository.UpdateAsync(mesa.Id,mesa);
                }

                if(entity.Estado == EstadoPedido.Finalizado)
                {
                    mesa.Estado = MesaEstado.Disponible;
                    await mesaRepository.UpdateAsync (mesa.Id,mesa);    
                }

                if(entity.Estado == EstadoPedido.Cancelado)
                {
                    mesa.Estado = MesaEstado.Disponible;
                    await mesaRepository.UpdateAsync(mesa.Id,mesa);
                }

                var platoIds = entity.DetallesPedidoDtos
                    .Select(x => x.PlatoId)
                    .Distinct()
                    .ToList();

                var platos = await platoRepository
                    .GetAllQuariableAsync()
                    .Where(x => platoIds.Contains(x.Id))
                    .ToListAsync();

                var platosDic = platos.ToDictionary(x => x.Id);

                var listDetalles = new List<DetallesPedido>();
                decimal total = 0m;

                foreach (var detalle in entity.DetallesPedidoDtos)
                {
                    var plato = platosDic[detalle.PlatoId];

                    total += plato.Precio * detalle.Cantidad;

                    listDetalles.Add(new DetallesPedido
                    {
                        PedidoId = pedido.Id,
                        PlatoId = plato.Id,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = plato.Precio,
                        Observaciones = detalle.Observaciones
                    });
                }

                var dataPedido = await pedidoRepository.UpdateAsync(
                    id,
                    new Pedido
                    {
                        Id = pedido.Id,
                        IdMesa = entity.IdMesa,
                        IdPropietario = pedido.IdPropietario,
                        Estado = entity.Estado,
                        FechaCreacion = pedido.FechaCreacion,
                        FechaActualizacion = DateTime.Now,
                        Total = total
                    });

                var dataDetalles = await detallesPedidoRepository.UpdateRangeAsync(id, listDetalles);

                return new MostrarPedidoConPlatosDto
                {
                    Id = dataPedido.Id,
                    IdMesa = dataPedido.IdMesa,
                    NombreMesa = mesa.Nombre,
                    TotalPedido = total,
                    Estado = dataPedido.Estado,
                    MostrarDetalles = dataDetalles
                        .Select(x => new MostrarDetallesPedidoDto
                        {
                            IdPlato = x.PlatoId,
                            CantidaPlato = x.Cantidad,
                            NombrePlato = x.Plato.Nombre,
                            PrecioPlato = x.PrecioUnitario,
                            Observaciones = x.Observaciones,
                        })
                        .ToList()
                };

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar actualizar el pedido" + ex.Message);



            }

        }






        public async Task<MostrarPedidoConPlatosDto> GetPedidoConPlatosByPedidoId(int pedidoId)
        {

            try
            {
                var pedido = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .FirstOrDefaultAsync(x => x.Id == pedidoId);

                if (pedido == null)
                {
                    return new MostrarPedidoConPlatosDto
                    {
                        HasError = true,
                        Errors = new List<string>
                {
                    "No se encontró el pedido."
                }
                    };
                }

                return new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(x => new MostrarDetallesPedidoDto
                        {
                            IdPlato = x.PlatoId,
                            NombrePlato = x.Plato.Nombre,
                            PrecioPlato = x.PrecioUnitario,
                            CantidaPlato = x.Cantidad,
                            Observaciones = x.Observaciones,
                        })
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener el pedido. " + ex.Message);
            }


        }






        public async Task<List<MostrarPedidoConPlatosDto>> GetPedidoConPlatosByPropietarioId(string propietarioId)
        {

            try
            {
                var pedidos = await pedidoRepository
                    .GetAllQuariableAsync()
                    .Include(x => x.Mesa)
                    .Include(x => x.DetallesPedidos)
                        .ThenInclude(x => x.Plato)
                    .Where(x => x.IdPropietario == propietarioId)
                    .ToListAsync();

                return pedidos.Select(pedido => new MostrarPedidoConPlatosDto
                {
                    Id = pedido.Id,
                    IdMesa = pedido.IdMesa,
                    NombreMesa = pedido.Mesa.Nombre,
                    TotalPedido = pedido.Total,
                    Estado = pedido.Estado,
                    MostrarDetalles = pedido.DetallesPedidos
                        .Select(detalle => new MostrarDetallesPedidoDto
                        {
                            IdPlato = detalle.PlatoId,
                            NombrePlato = detalle.Plato.Nombre,
                            PrecioPlato = detalle.PrecioUnitario,
                            CantidaPlato = detalle.Cantidad,
                            Observaciones = detalle.Observaciones,
                        })
                        .ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar obtener los pedidos del propietario. " + ex.Message);
            }


        }





        #region private method
        private async Task<ResponsePedidoDto> ValidarYActualizarStockAsync(CreatePedidoDto dto)
        {
            ResponsePedidoDto response = new();


            var platoIds = dto.DetallesPedidoDtos
                .Select(x => x.PlatoId)
                .Distinct()
                .ToList();


            var platosProductos = await platoProductoRepository
                .GetAllQuariableAsync()
                .Where(x => platoIds.Contains(x.PlatoId))
                .ToListAsync();

            if (!platosProductos.Any())
            {
                response.HasError = true;
                response.Errors.Add("No se encontraron los ingredientes de los platos del pedido.");
                return response;
            }


            var productoIds = platosProductos
                .Select(x => x.ProductoId)
                .Distinct()
                .ToList();

            var productos = await productoRepository.GetByIdsAsync(productoIds);

            var productosDic = productos.ToDictionary(x => x.Id);


            Dictionary<int, decimal> productosNecesarios = new();

            foreach (var detalle in dto.DetallesPedidoDtos)
            {
                var ingredientes = platosProductos
                    .Where(x => x.PlatoId == detalle.PlatoId);

                foreach (var ingrediente in ingredientes)
                {
                    if (!productosNecesarios.ContainsKey(ingrediente.ProductoId))
                    {
                        productosNecesarios[ingrediente.ProductoId] = 0;
                    }

                    productosNecesarios[ingrediente.ProductoId] +=
                        ingrediente.Cantidad * detalle.Cantidad;
                }
            }


            foreach (var item in productosNecesarios)
            {
                if (!productosDic.TryGetValue(item.Key, out var producto))
                {
                    response.HasError = true;
                    response.Errors.Add($"No se encontró el producto con Id {item.Key}.");
                    return response;
                }

                if (producto.StockActual < item.Value)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        $"Stock insuficiente para el producto '{producto.Nombre}'. " +
                        $"Se requieren {item.Value} {producto.UnidadMedida} y solo hay {producto.StockActual}."
                    );

                    return response;
                }
            }


            foreach (var item in productosNecesarios)
            {
                productosDic[item.Key].StockActual -= item.Value;
            }


            var actualizado = await productoRepository.UpdateRangeAsync(productosDic.Values.ToList());

            if (actualizado == null || !actualizado.Any())
            {
                response.HasError = true;
                response.Errors.Add("Ocurrió un error al actualizar el inventario.");
                return response;
            }

            response.HasError = false;
            return response;
        }



        private async Task<ResponsePedidoDto> ValidarYActualizarStockUpdateAsync(
         EditarPedidoDto dto,
          List<DetallesPedido> detallesActuales)
        {
            ResponsePedidoDto response = new();


            var platoIds = detallesActuales
                .Select(x => x.PlatoId)
                .Union(dto.DetallesPedidoDtos.Select(x => x.PlatoId))
                .Distinct()
                .ToList();

            var platosProductos = await platoProductoRepository
                .GetAllQuariableAsync()
                .Where(x => platoIds.Contains(x.PlatoId))
                .ToListAsync();

            if (!platosProductos.Any())
            {
                response.HasError = true;
                response.Errors.Add("No se encontraron los ingredientes de los platos.");
                return response;
            }


            var productoIds = platosProductos
                .Select(x => x.ProductoId)
                .Distinct()
                .ToList();

            var productos = await productoRepository.GetByIdsAsync(productoIds);

            var productosDic = productos.ToDictionary(x => x.Id);


            Dictionary<int, decimal> consumoActual = new();

            foreach (var detalle in detallesActuales)
            {
                var ingredientes = platosProductos.Where(x => x.PlatoId == detalle.PlatoId);

                foreach (var ingrediente in ingredientes)
                {
                    if (!consumoActual.ContainsKey(ingrediente.ProductoId))
                        consumoActual[ingrediente.ProductoId] = 0;

                    consumoActual[ingrediente.ProductoId] +=
                        ingrediente.Cantidad * detalle.Cantidad;
                }
            }


            Dictionary<int, decimal> consumoNuevo = new();

            foreach (var detalle in dto.DetallesPedidoDtos)
            {
                var ingredientes = platosProductos.Where(x => x.PlatoId == detalle.PlatoId);

                foreach (var ingrediente in ingredientes)
                {
                    if (!consumoNuevo.ContainsKey(ingrediente.ProductoId))
                        consumoNuevo[ingrediente.ProductoId] = 0;

                    consumoNuevo[ingrediente.ProductoId] +=
                        ingrediente.Cantidad * detalle.Cantidad;
                }
            }


            var todosLosProductos = consumoActual.Keys
                .Union(consumoNuevo.Keys);

            foreach (var productoId in todosLosProductos)
            {
                decimal cantidadActual = consumoActual.ContainsKey(productoId)
                    ? consumoActual[productoId]
                    : 0;

                decimal cantidadNueva = consumoNuevo.ContainsKey(productoId)
                    ? consumoNuevo[productoId]
                    : 0;

                decimal diferencia = cantidadNueva - cantidadActual;

                if (!productosDic.TryGetValue(productoId, out var producto))
                {
                    response.HasError = true;
                    response.Errors.Add($"No se encontró el producto con Id {productoId}.");
                    return response;
                }

                if (diferencia > 0 && producto.StockActual < diferencia)
                {
                    response.HasError = true;
                    response.Errors.Add(
                        $"No hay suficiente stock del producto '{producto.Nombre}'. " +
                        $"Se necesitan {diferencia} {producto.UnidadMedida} adicionales y solo hay {producto.StockActual}."
                    );

                    return response;
                }
            }


            foreach (var productoId in todosLosProductos)
            {
                decimal cantidadActual = consumoActual.ContainsKey(productoId)
                    ? consumoActual[productoId]
                    : 0;

                decimal cantidadNueva = consumoNuevo.ContainsKey(productoId)
                    ? consumoNuevo[productoId]
                    : 0;

                decimal diferencia = cantidadNueva - cantidadActual;

                productosDic[productoId].StockActual -= diferencia;
            }

            var actualizado = await productoRepository
                .UpdateRangeAsync(productosDic.Values.ToList());

            if (actualizado == null || !actualizado.Any())
            {
                response.HasError = true;
                response.Errors.Add("Ocurrió un error al actualizar el inventario.");
                return response;
            }

            response.HasError = false;
            return response;
        }

        #endregion





    }
}
