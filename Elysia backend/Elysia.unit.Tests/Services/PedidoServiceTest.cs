using AutoMapper;
using Elysia.Core.Application.Dtos.pedido;
using Elysia.Core.Application.Mapping.EntityToDtoMappingProfile;
using Elysia.Core.Application.Services;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Infraestructure.persistences.Contexts;
using Elysia.Infraestructure.persistences.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.unit.Tests.Services
{
    public class PedidoServiceTest
    {

        private readonly IMapper mapper;
        private readonly DbContextOptions<ElysiaContext> _dbContextOptions;
        private ElysiaContext _context = null!;

        public PedidoServiceTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"pedidoServiceDbInMemory_{Guid.NewGuid()}")
                .Options;

            _context = new ElysiaContext(_dbContextOptions);

            var loggerFactory = LoggerFactory.Create(cfg =>
            {
                cfg.AddConsole();
            });

            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile<PedidoEntityToDtoMappingProfile>();
            }, loggerFactory);

            mapper = config.CreateMapper();
        }

        #region private method

        public PedidoService CreateService()
        {
            var pedidoRepository = new PedidoRepository(_context);
            var productoRepository = new ProductoRepository(_context);
            var platoProductoRepository = new PlatoProductoRepository(_context);
            var reservasRepository = new ReservasRepository(_context);
            var mesaRepository = new MesaRepository(_context);
            var platoRepository = new PlatoRepository(_context);
            var detallesPedidoRepository = new DetallesPedidoRepository(_context);

            return new PedidoService(
                pedidoRepository,
                productoRepository,
                platoProductoRepository,
                reservasRepository,
                mesaRepository,
                platoRepository,
                detallesPedidoRepository,
                mapper);
        }

        #endregion


        [Fact]
        public async Task AddAsync_should_return_add_dto()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });


            var productoRepository = new ProductoRepository(_context);

            var producto = await productoRepository.AddAsync(new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Pechuga de pollo",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            });


            var platoRepository = new PlatoRepository(_context);

            var plato = await platoRepository.AddAsync(new Plato
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            });


            var platoProductoRepository = new PlatoProductoRepository(_context);

            await platoProductoRepository.AddAsync(new PlatoProducto
            {
                PlatoId = plato.Id,
                ProductoId = producto.Id,
                Cantidad = 1
            });


            var pedido = new CreatePedidoDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = mesa.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,

                DetallesPedidoDtos = new List<CreateDetallesPedidoRequestDto>
            {
                new CreateDetallesPedidoRequestDto
                {
                    PlatoId = plato.Id,
                    Cantidad = 1,
                    Observaciones = "Sin cebolla"
                }
            }
            };

            // act
            var act = await service.AddAsync(pedido);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.IdMesa.Should().Be(pedido.IdMesa);
        }


        [Fact]
        public async Task AddAsync_should_return_response_when_not_add()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.AddAsync(null);

            // assert
            act.Should().NotBeNull();
            act.HasError.Should().BeTrue();
        }


        [Fact]
        public async Task UpdateAsync_should_return_new_Pedido()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });


            var productoRepository = new ProductoRepository(_context);

            var producto = await productoRepository.AddAsync(new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Pechuga de pollo",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            });


            var platoRepository = new PlatoRepository(_context);

            var plato = await platoRepository.AddAsync(new Plato
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            });


            var platoProductoRepository = new PlatoProductoRepository(_context);

            await platoProductoRepository.AddAsync(new PlatoProducto
            {
                PlatoId = plato.Id,
                ProductoId = producto.Id,
                Cantidad = 1
            });


            var pedido = new CreatePedidoDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = mesa.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,

                DetallesPedidoDtos = new List<CreateDetallesPedidoRequestDto>
            {
                new CreateDetallesPedidoRequestDto
                {
                    PlatoId = plato.Id,
                    Cantidad = 1,
                    Observaciones = "Sin cebolla"
                }
            }
            };

            var add = await service.AddAsync(pedido);

            var map = mapper.Map<EditarPedidoDto>(add);

            map.Estado = EstadoPedido.Listo;
            map.Total = 450;
            map.FechaActualizacion = DateTime.Now;

            // act
            var act = await service.UpdateAsync(add.Id, map);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(add.Id);
            act.Estado.Should().Be(map.Estado);
            act.IdMesa.Should().Be(map.IdMesa);
        }


        [Fact]
        public async Task UpdateAsync_should_return_response_when_not_add()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.UpdateAsync(999, null);

            // assert
            act.Should().NotBeNull();
            act.HasError.Should().BeTrue();
        }


        [Fact]
        public async Task GetByIdAsync_should_return_pedido_when_exist()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });


            var productoRepository = new ProductoRepository(_context);

            var producto = await productoRepository.AddAsync(new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Pechuga de pollo",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            });


            var platoRepository = new PlatoRepository(_context);

            var plato = await platoRepository.AddAsync(new Plato
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            });


            var platoProductoRepository = new PlatoProductoRepository(_context);

            await platoProductoRepository.AddAsync(new PlatoProducto
            {
                PlatoId = plato.Id,
                ProductoId = producto.Id,
                Cantidad = 1
            });


            var pedido = new CreatePedidoDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = mesa.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,

                DetallesPedidoDtos = new List<CreateDetallesPedidoRequestDto>
            {
                new CreateDetallesPedidoRequestDto
                {
                    PlatoId = plato.Id,
                    Cantidad = 1,
                    Observaciones = "Sin cebolla"
                }
            }
            };

            var add = await service.AddAsync(pedido);

            // act
            var act = await service.GetByIdAsync(add.Id);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(add.Id);
            act.IdMesa.Should().Be(pedido.IdMesa);
        }


        [Fact]
        public async Task GetByIdAsync_should_return_null_when_pedido_not_exist()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }


        [Fact]
        public async Task DeleteIdAsync_should_return_true_when_pedido_deleted()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });


            var productoRepository = new ProductoRepository(_context);

            var producto = await productoRepository.AddAsync(new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Pechuga de pollo",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            });


            var platoRepository = new PlatoRepository(_context);

            var plato = await platoRepository.AddAsync(new Plato
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            });


            var platoProductoRepository = new PlatoProductoRepository(_context);

            await platoProductoRepository.AddAsync(new PlatoProducto
            {
                PlatoId = plato.Id,
                ProductoId = producto.Id,
                Cantidad = 1
            });


            var pedido = new CreatePedidoDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = mesa.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,

                DetallesPedidoDtos = new List<CreateDetallesPedidoRequestDto>
            {
                new CreateDetallesPedidoRequestDto
                {
                    PlatoId = plato.Id,
                    Cantidad = 1,
                    Observaciones = "Sin cebolla"
                }
            }
            };

            var add = await service.AddAsync(pedido);

            // act
            var act = await service.DeleteAsync(add.Id);

            // assert
            act.Should().BeTrue();
        }


        [Fact]
        public async Task DeleteIdAsync_should_return_false_when_pedido_not_deleted()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }


        [Fact]
        public async Task GetAllAsync_should_return_list_when_find_pedidos()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa1 = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });


            var productoRepository = new ProductoRepository(_context);

            var producto1 = await productoRepository.AddAsync(new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Pechuga de pollo",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto1.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            });


            var platoRepository = new PlatoRepository(_context);

            var plato1 = await platoRepository.AddAsync(new Plato
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato1.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            });


            var platoProductoRepository = new PlatoProductoRepository(_context);

            await platoProductoRepository.AddAsync(new PlatoProducto
            {
                PlatoId = plato1.Id,
                ProductoId = producto1.Id,
                Cantidad = 1
            });


            var pedido1 = new CreatePedidoDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = mesa1.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,

                DetallesPedidoDtos = new List<CreateDetallesPedidoRequestDto>
            {
                new CreateDetallesPedidoRequestDto
                {
                    PlatoId = plato1.Id,
                    Cantidad = 1,
                    Observaciones = "Sin cebolla"
                }
            }
            };

            var add1 = await service.AddAsync(pedido1);


            var mesa2 = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 2",
                Descripcion = "Mesa para seis personas",
                Capacidad = 6,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });


            var producto2 = await productoRepository.AddAsync(new Producto
            {
                Nombre = "Arroz",
                Descripcion = "Arroz blanco",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto2.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            });


            var plato2 = await platoRepository.AddAsync(new Plato
            {
                Nombre = "Arroz con pollo",
                Descripcion = "Plato de arroz con pollo",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 350,
                Imagen = "https://example.com/plato2.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-002",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            });


            await platoProductoRepository.AddAsync(new PlatoProducto
            {
                PlatoId = plato2.Id,
                ProductoId = producto2.Id,
                Cantidad = 1
            });


            var pedido2 = new CreatePedidoDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = mesa2.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,

                DetallesPedidoDtos = new List<CreateDetallesPedidoRequestDto>
            {
                new CreateDetallesPedidoRequestDto
                {
                    PlatoId = plato2.Id,
                    Cantidad = 2,
                    Observaciones = "Extra arroz"
                }
            }
            };

            var add2 = await service.AddAsync(pedido2);


            var mesa3 = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 3",
                Descripcion = "Mesa para ocho personas",
                Capacidad = 8,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });


            var producto3 = await productoRepository.AddAsync(new Producto
            {
                Nombre = "Pan",
                Descripcion = "Pan de la casa",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Unidad",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto3.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            });


            var plato3 = await platoRepository.AddAsync(new Plato
            {
                Nombre = "Hamburguesa",
                Descripcion = "Hamburguesa de carne",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 500,
                Imagen = "https://example.com/plato3.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-003",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            });


            await platoProductoRepository.AddAsync(new PlatoProducto
            {
                PlatoId = plato3.Id,
                ProductoId = producto3.Id,
                Cantidad = 1
            });


            var pedido3 = new CreatePedidoDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = mesa3.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,

                DetallesPedidoDtos = new List<CreateDetallesPedidoRequestDto>
            {
                new CreateDetallesPedidoRequestDto
                {
                    PlatoId = plato3.Id,
                    Cantidad = 3,
                    Observaciones = "Sin tomate"
                }
            }
            };

            var add3 = await service.AddAsync(pedido3);

            // act
            var act = await service.GetlAllAsync();

            // assert
            act.Should().HaveCount(3);
            act.Should().NotBeEmpty();
        }


        [Fact]
        public async Task GetAllAsync_should_return_empty_when_pedidos_not_found()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.GetlAllAsync();

            // assert
            act.Should().BeEmpty();
        }














    }
}
