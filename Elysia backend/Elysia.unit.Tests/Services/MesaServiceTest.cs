using AutoMapper;
using Elysia.Core.Application.Dtos.Mesa;
using Elysia.Core.Application.Mapping.EntityToDtoMappingProfile;
using Elysia.Core.Application.Services;
using Elysia.Core.Domain.Common;
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
    public class MesaServiceTest
    {



        private readonly IMapper mapper;
        private readonly DbContextOptions<ElysiaContext> _dbContextOptions;




        public MesaServiceTest()
        {

            _dbContextOptions = new DbContextOptionsBuilder<ElysiaContext>()
                                    .UseInMemoryDatabase(databaseName: $"mesaServiceDbInMemory_{Guid.NewGuid()}")
                                    .Options;


            var loggerFactory = LoggerFactory.Create(cfg =>
            {


                cfg.AddConsole();

            });


            var config = new MapperConfiguration(opt =>
            {

                opt.AddProfile<MesaEntityToMappingProfile>();



            }, loggerFactory);

            mapper = config.CreateMapper();

        }



        #region private method
        public MesaService CreateService()
        {

            var context = new ElysiaContext(_dbContextOptions);
            var repo = new MesaRepository(context);

            var service = new MesaService(repo, mapper);
            return service;

        }
        #endregion



        [Fact]
        public async Task AddAsync_should_return_add_dto()
        {
            // arrange
            var service = CreateService();

            var mesa = new CreateMesaDto
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://example.com/mesa1.jpg",
                Codigo = "MESA-001"
            };

            // act
            var act = await service.AddAsync(mesa);

            // assert
            act.Should().NotBeNull();
            act.HasError.Should().BeFalse();
            act.Nombre.Should().Be(mesa.Nombre);
            act.Descripcion.Should().Be(mesa.Descripcion);
            act.Capacidad.Should().Be(mesa.Capacidad);
            act.Codigo.Should().Be(mesa.Codigo);
            act.Id.Should().BeGreaterThan(0);
        }



        [Fact]
        public async Task AddAsync_should_return_null_when_not_add()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.AddAsync(null);

            // assert
            act.Should().BeNull();
        }


        [Fact]
        public async Task UpdateAsync_should_return_new_Mesa()
        {
            // arrange
            var service = CreateService();

            var mesa = new CreateMesaDto
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://example.com/mesa1.jpg",
                Codigo = "MESA-001"
            };

            // Primero agregamos la mesa
            var add = await service.AddAsync(mesa);

          
            // Convertimos a DTO de edición
            var edit = mapper.Map<EditarMesaDto>(add);

            // Cambiamos algunos datos
            edit.Nombre = "Mesa VIP";
            edit.Descripcion = "Mesa especial para clientes VIP";
            edit.Capacidad = 6;

            // act
            var act = await service.UpdateAsync(add.Id, edit);

            // assert
            act.Should().NotBeNull();
            act.HasError.Should().BeFalse();
            act.Id.Should().Be(add.Id);
            act.Nombre.Should().Be(edit.Nombre);
            act.Descripcion.Should().Be(edit.Descripcion);
            act.Capacidad.Should().Be(edit.Capacidad);
        }




        [Fact]
        public async Task UpdateAsync_should_return_response_when_not_updated()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.UpdateAsync(999, null);

            // assert
            act.Should().NotBeNull();
            act.HasError.Should().BeTrue();
            act.Errors.Should().NotBeNullOrEmpty();
        }


        [Fact]
        public async Task GetByIdAsync_should_return_mesa_when_exist()
        {
            // arrange
            var service = CreateService();

            var mesa = new CreateMesaDto
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://example.com/mesa1.jpg",
                Codigo = "MESA-001"
            };

            var add = await service.AddAsync(mesa);

            add.Should().NotBeNull();
            add.HasError.Should().BeFalse();

            // act
            var act = await service.GetByIdAsync(add.Id);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(add.Id);
            act.Nombre.Should().Be(add.Nombre);
            act.Descripcion.Should().Be(add.Descripcion);
            act.Capacidad.Should().Be(add.Capacidad);
            act.Codigo.Should().Be(add.Codigo);
        }


        [Fact]
        public async Task GetByIdAsync_should_return_null_when_mesa_not_exist()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }


        [Fact]
        public async Task DeleteAsync_should_return_true_when_mesa_deleted()
        {
            // arrange
            var service = CreateService();

            var mesa = new CreateMesaDto
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://example.com/mesa1.jpg",
                Codigo = "MESA-001"
            };

            var add = await service.AddAsync(mesa);

            add.Should().NotBeNull();
            add.HasError.Should().BeFalse();

            // act
            var act = await service.DeleteAsync(add.Id);

            // assert
            act.Should().BeTrue();
        }


        [Fact]
        public async Task DeleteAsync_should_return_false_when_mesa_not_deleted()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }


        [Fact]
        public async Task GetAllAsync_should_return_list_when_find_mesas()
        {
            // arrange
            var service = CreateService();

            var mesa1 = new CreateMesaDto
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa para cuatro personas",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://example.com/mesa1.jpg",
                Codigo = "MESA-001"
            };

            var mesa2 = new CreateMesaDto
            {
                Nombre = "Mesa 2",
                Descripcion = "Mesa para dos personas",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 2,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://example.com/mesa2.jpg",
                Codigo = "MESA-002"
            };

            var mesa3 = new CreateMesaDto
            {
                Nombre = "Mesa 3",
                Descripcion = "Mesa para seis personas",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 6,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://example.com/mesa3.jpg",
                Codigo = "MESA-003"
            };

            await service.AddAsync(mesa1);
            await service.AddAsync(mesa2);
            await service.AddAsync(mesa3);

            // act
            var act = await service.GetlAllAsync();

            // assert
            act.Should().NotBeEmpty();
            act.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetAllAsync_should_return_empty_when_mesas_not_found()
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
