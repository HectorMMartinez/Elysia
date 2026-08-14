using AutoMapper;
using Elysia.Core.Application.Dtos.plato;
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

    public class PlatoServiceTest
    {
        private readonly IMapper mapper;
        private readonly DbContextOptions<ElysiaContext> _dbContextOptions;
        private ElysiaContext _context = null!;

        public PlatoServiceTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"platoServiceDbInMemory_{Guid.NewGuid()}")
                .Options;

            _context = new ElysiaContext(_dbContextOptions);

            var loggerFactory = LoggerFactory.Create(cfg =>
            {
                cfg.AddConsole();
            });

            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile<PlatoEntityToDtoMappingProfile>();
            }, loggerFactory);

            mapper = config.CreateMapper();
        }

        #region private method
        public PlatoService CreateService()
        {
            var platoRepo = new PlatoRepository(_context);
            var platoProductoRepo = new PlatoProductoRepository(_context);
            var categoriaRepo = new CategoriaPlatoRepository(_context);
            var productoRepo = new ProductoRepository(_context);
            var platoMenuRepo = new PlatoMenuRepository(_context);

            return new PlatoService(
                platoMenuRepo,
                platoRepo,
                platoProductoRepo,
                categoriaRepo,
                productoRepo,
                mapper);
        }

        private CategoriaPlatoRepository RepoCategoria()
        {
            var repo = new CategoriaPlatoRepository(_context);
            return repo;
        }
        #endregion


        [Fact]
        public async Task AddAsync_should_return_add_dto()
        {
            // arrange
            var service = CreateService();
            var repoCategoria = RepoCategoria();

            var categoria = await repoCategoria.AddAsync(new CategoriaPlato()
            {
                Nombre = "Carne",
                Descripcion = "Es un tipo de alimento alto en proteina"
            });

            // producto requerido por la lógica del service
            var producto = new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Insumo de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.Set<Producto>().Add(producto);
            await _context.SaveChangesAsync();

            var plato = new CreatePlatoDto()
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = categoria.Id,
                Estado = PlatoEstado.Disponible,

                // ⚠️ usa el nombre REAL de tu DTO
                ProductoQuantityDtos = new List<productoQuantityDto>
           {
              new productoQuantityDto
            {
                Id = producto.Id,
                Cantidad = 1
            }
        }
            };

            // act
            var act = await service.AddAsync(plato);

            // assert
            act.Should().NotBeNull();
            act.HasError.Should().BeFalse($"Errors: {string.Join(", ", act.Errors ?? new List<string>())}");
            act.Nombre.Should().Be(plato.Nombre);
            act.Descripcion.Should().Be(plato.Descripcion);
            act.Id.Should().BeGreaterThan(0);
        }


        [Fact]
        public async Task AddAsync_should_return_response_when_not_add()
        {
            //arrange
            var service = CreateService();

            //act
            var act = await service.AddAsync(null);

            //assert
            act.Should().NotBeNull();
            act.HasError.Should().BeTrue();
        }



        [Fact]
        public async Task UpdateAsync_should_return_new_Plato()
        {
            // arrange
            var service = CreateService();
            var repoCategoria = RepoCategoria();

            var categoria = await repoCategoria.AddAsync(new CategoriaPlato()
            {
                Nombre = "Carne",
                Descripcion = "Es un tipo de alimento alto en proteina"
            });


            // producto requerido por la lógica del service
            var producto = new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Insumo de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.Set<Producto>().Add(producto);
            await _context.SaveChangesAsync();

            var plato = new CreatePlatoDto()
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = categoria.Id,
                Estado = PlatoEstado.Disponible,

                // ⚠️ usa el nombre REAL de tu DTO
                ProductoQuantityDtos = new List<productoQuantityDto>
           {
              new productoQuantityDto
            {
                Id = producto.Id,
                Cantidad = 1
            }
        }
            };

            var add = await service.AddAsync(plato);

            var map = mapper.Map<EditarPlatoDto>(add);

            map.Nombre = "Hamburguesa de carne";

            // act
            var act = await service.UpdateAsync(add.Id, map);

            // assert
            act.Should().NotBeNull();
            act.Nombre.Should().Be(map.Nombre);
            act.Id.Should().BeGreaterThan(0);
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
        public async Task GetByIdAsync_should_return_plato_when_exist()
        {
            // arrange
            var service = CreateService();
            var repoCategoria = RepoCategoria();

            var categoria = await repoCategoria.AddAsync(new CategoriaPlato()
            {
                Nombre = "Carne",
                Descripcion = "Es un tipo de alimento alto en proteina"
            });

            // producto requerido por la lógica del service
            var producto = new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Insumo de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.Set<Producto>().Add(producto);
            await _context.SaveChangesAsync();

            var plato = new CreatePlatoDto()
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = categoria.Id,
                Estado = PlatoEstado.Disponible,

                // ⚠️ usa el nombre REAL de tu DTO
                ProductoQuantityDtos = new List<productoQuantityDto>
           {
              new productoQuantityDto
            {
                Id = producto.Id,
                Cantidad = 1
            }
        }
            };

            // Primero agregamos el plato
            var add = await service.AddAsync(plato);

            // act
            var act = await service.GetByIdAsync(add.Id);

            // assert
            act.Should().NotBeNull();
            act.Nombre.Should().Be(plato.Nombre);
            act.Id.Should().Be(add.Id);
        }


        [Fact]
        public async Task GetByIdAsync_should_return_null_when_plato_not_exist()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }


        [Fact]
        public async Task DeleteIdAsync_should_return_true_when_plato_deleted()
        {
            // arrange
            var service = CreateService();
            var repoCategoria = RepoCategoria();

            var categoria = await repoCategoria.AddAsync(new CategoriaPlato()
            {
                Nombre = "Carne",
                Descripcion = "Es un tipo de alimento alto en proteina"
            });

            // producto requerido por la lógica del service
            var producto = new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Insumo de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.Set<Producto>().Add(producto);
            await _context.SaveChangesAsync();

            var plato = new CreatePlatoDto()
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = categoria.Id,
                Estado = PlatoEstado.Disponible,

                // ⚠️ usa el nombre REAL de tu DTO
                ProductoQuantityDtos = new List<productoQuantityDto>
           {
              new productoQuantityDto
            {
                Id = producto.Id,
                Cantidad = 1
            }
        }
            };


            var add = await service.AddAsync(plato);

            // act
            var act = await service.DeleteAsync(add.Id);

            // assert
            act.Should().BeTrue();
        }


        [Fact]
        public async Task DeleteIdAsync_should_return_false_when_plato_not_deleted()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }


        [Fact]
        public async Task GetAllAsync_should_return_list_when_find_platos()
        {
            // arrange
            var service = CreateService();
            var repoCategoria = RepoCategoria();


            var categoria = await repoCategoria.AddAsync(new CategoriaPlato()
            {
                Nombre = "Carne",
                Descripcion = "Es un tipo de alimento alto en proteina"
            });

            // producto requerido por la lógica del service
            var producto = new Producto
            {
                Nombre = "Pechuga",
                Descripcion = "Insumo de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.Set<Producto>().Add(producto);
            await _context.SaveChangesAsync();

            var plato1 = new CreatePlatoDto()
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = categoria.Id,
                Estado = PlatoEstado.Disponible,

                // ⚠️ usa el nombre REAL de tu DTO
                ProductoQuantityDtos = new List<productoQuantityDto>
           {
              new productoQuantityDto
            {
                Id = producto.Id,
                Cantidad = 1
            }
        }
            };




            // producto requerido por la lógica del service
            var producto2 = new Producto
            {
                Nombre = "Arroz",
                Descripcion = "Insumo de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.Set<Producto>().Add(producto2);
            await _context.SaveChangesAsync();

            var plato2 = new CreatePlatoDto()
            {
                Nombre = "Pechuga a la plancha",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = categoria.Id,
                Estado = PlatoEstado.Disponible,

                // ⚠️ usa el nombre REAL de tu DTO
                ProductoQuantityDtos = new List<productoQuantityDto>
           {
              new productoQuantityDto
            {
                Id = producto2.Id,
                Cantidad = 2
            }
        }
            };



           

            // producto requerido por la lógica del service
            var producto3 = new Producto
            {
                Nombre = "Pan",
                Descripcion = "Insumo de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 10,
                Imagen = "https://example.com/producto.jpg",
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            _context.Set<Producto>().Add(producto3);
            await _context.SaveChangesAsync();

            var plato3 = new CreatePlatoDto()
            {
                Nombre = "Hamburguesa de pollo",
                Descripcion = "Plato de prueba",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 450,
                Imagen = "https://example.com/plato.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Codigo = "PLT-001",
                CategoriaId = categoria.Id,
                Estado = PlatoEstado.Disponible,

                // ⚠️ usa el nombre REAL de tu DTO
                ProductoQuantityDtos = new List<productoQuantityDto>
           {
              new productoQuantityDto
            {
                Id = producto3.Id,
                Cantidad = 3
            }
        }
            };



            var add1 = await service.AddAsync(plato1);
            var add2 = await service.AddAsync(plato2);
            var add3 = await service.AddAsync(plato3);


            // act
            var act = await service.GetlAllAsync();

            // assert
            act.Should().HaveCount(3);
            act.Should().NotBeEmpty();
        }


        [Fact]
        public async Task GetAllAsync_should_return_empty_when_platos_not_found()
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

