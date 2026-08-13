using AutoMapper;
using Elysia.Core.Application.Dtos.reservas;
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
    public class ReservaServiceTest
    {

        private readonly IMapper mapper;
        private readonly DbContextOptions<ElysiaContext> _dbContextOptions;
        private ElysiaContext _context = null!;

        public ReservaServiceTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"reservaServiceDbInMemory_{Guid.NewGuid()}")
                .Options;

            _context = new ElysiaContext(_dbContextOptions);

            var loggerFactory = LoggerFactory.Create(cfg =>
            {
                cfg.AddConsole();
            });

            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile<ReservaEntityToMappingProfile>();
            }, loggerFactory);

            mapper = config.CreateMapper();
        }

        #region private method

        public ReservaServices CreateService()
        {
            var reservaRepo = new ReservasRepository(_context);
            var mesaRepository = new MesaRepository(_context);

            return new ReservaServices(
                reservaRepo,
                mesaRepository,
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
                Descripcion = "Mesa de prueba",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });

            var reserva = new CreateReservaDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Perez",
                DNICliente = "00112345678",
                MesaId = mesa.Id,
                CantidadPersona = 2,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Reserva de prueba"
            };

            // act
            var act = await service.AddAsync(reserva);

            // assert
            act.Should().NotBeNull();
            act.NombreCliente.Should().Be(reserva.NombreCliente);
            act.DNICliente.Should().Be(reserva.DNICliente);
            act.MesaId.Should().Be(reserva.MesaId);
            act.CantidadPersona.Should().Be(reserva.CantidadPersona);
            act.Id.Should().BeGreaterThan(0);
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
        public async Task UpdateAsync_should_return_new_Reserva()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa de prueba",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });

            var reserva = new CreateReservaDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Perez",
                DNICliente = "00112345678",
                MesaId = mesa.Id,
                CantidadPersona = 2,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Reserva de prueba"
            };

            var add = await service.AddAsync(reserva);

            var map = mapper.Map<EditarReservaDto>(add);

            map.NombreCliente = "Pedro Martinez";
            map.Observaciones = "Reserva actualizada";

            // act
            var act = await service.UpdateAsync(add.Id, map);

            // assert
            act.Should().NotBeNull();
            act.NombreCliente.Should().Be(map.NombreCliente);
            act.Observaciones.Should().Be(map.Observaciones);
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
        public async Task GetByIdAsync_should_return_reserva_when_exist()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa de prueba",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });

            var reserva = new CreateReservaDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Perez",
                DNICliente = "00112345678",
                MesaId = mesa.Id,
                CantidadPersona = 2,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Reserva de prueba"
            };

            var add = await service.AddAsync(reserva);

            // act
            var act = await service.GetByIdAsync(add.Id);

            // assert
            act.Should().NotBeNull();
            act.NombreCliente.Should().Be(reserva.NombreCliente);
            act.DNICliente.Should().Be(reserva.DNICliente);
            act.MesaId.Should().Be(reserva.MesaId);
            act.Id.Should().Be(add.Id);
        }


        [Fact]
        public async Task GetByIdAsync_should_return_null_when_reserva_not_exist()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }



        [Fact]
        public async Task DeleteIdAsync_should_return_true_when_reserva_deleted()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa de prueba",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });

            var reserva = new CreateReservaDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Perez",
                DNICliente = "00112345678",
                MesaId = mesa.Id,
                CantidadPersona = 2,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Reserva de prueba"
            };

            var add = await service.AddAsync(reserva);

            // act
            var act = await service.DeleteAsync(add.Id);

            // assert
            act.Should().BeTrue();
        }




        [Fact]
        public async Task DeleteIdAsync_should_return_false_when_reserva_not_deleted()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }



        [Fact]
        public async Task GetAllAsync_should_return_list_when_find_reservas()
        {
            // arrange
            var service = CreateService();

            var mesaRepository = new MesaRepository(_context);

            var mesa1 = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa de prueba 1",
                Capacidad = 4,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });

            var reserva1 = new CreateReservaDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Perez",
                DNICliente = "00112345678",
                MesaId = mesa1.Id,
                CantidadPersona = 2,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Reserva uno"
            };

            var add1 = await service.AddAsync(reserva1);


            var mesa2 = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 2",
                Descripcion = "Mesa de prueba 2",
                Capacidad = 6,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });

            var reserva2 = new CreateReservaDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Pedro Martinez",
                DNICliente = "00112345679",
                MesaId = mesa2.Id,
                CantidadPersona = 4,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(2),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Reserva dos"
            };

            var add2 = await service.AddAsync(reserva2);


            var mesa3 = await mesaRepository.AddAsync(new Mesa
            {
                Nombre = "Mesa 3",
                Descripcion = "Mesa de prueba 3",
                Capacidad = 8,
                Estado = MesaEstado.Disponible,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            });

            var reserva3 = new CreateReservaDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Carlos Rodriguez",
                DNICliente = "00112345680",
                MesaId = mesa3.Id,
                CantidadPersona = 6,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(3),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Reserva tres"
            };

            var add3 = await service.AddAsync(reserva3);

            // act
            var act = await service.GetlAllAsync();

            // assert
            act.Should().HaveCount(3);
            act.Should().NotBeEmpty();
        }


        [Fact]
        public async Task GetAllAsync_should_return_empty_when_reservas_not_found()
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
