using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using Elysia.Infraestructure.persistences.Contexts;
using Elysia.Infraestructure.persistences.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Integration.Tests.Persistences.Repositories
{
    public class ReservaRepositoryTest
    {
        private readonly DbContextOptions<ElysiaContext> _dbContextOption;


        public ReservaRepositoryTest()
        {

            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"reservaDbMemory_{Guid.NewGuid()}").Options;

        }


        [Fact]
        public async Task AddAsync_Should_Add_Reserva_To_Database()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            var reserva = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Pérez",
                DNICliente = "00123456789",
                MesaId = 1,
                CantidadPersona = 4,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Cliente prefiere mesa cerca de la ventana"
            };

            // act
            var act = await repo.AddAsync(reserva);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.NombreCliente.Should().Be(reserva.NombreCliente);
            act.DNICliente.Should().Be(reserva.DNICliente);
            act.CantidadPersona.Should().Be(4);
            act.Estado.Should().Be(EstadoReserva.Activa);

            var data = context.Reservas.ToList();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Exception_When_Not_Add_Reserva()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            // act
            Func<Task> act = async () => await repo.AddAsync(null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Reserva()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            var reserva = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Pérez",
                DNICliente = "00123456789",
                MesaId = 1,
                CantidadPersona = 4,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Cliente prefiere mesa cerca de la ventana"
            };

            var added = await repo.AddAsync(reserva);

            // modificamos
            added.NombreCliente = "María Gómez";
            added.CantidadPersona = 6;
            added.Estado = EstadoReserva.EnProceso;
            added.FechaActualizacion = DateTime.Now;

            // act
            var act = await repo.UpdateAsync(added.Id, added);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(added.Id);
            act.NombreCliente.Should().Be("María Gómez");
            act.CantidadPersona.Should().Be(6);
            act.Estado.Should().Be(EstadoReserva.EnProceso);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Exception_When_Not_Updated_Reserva()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            var reserva = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Pérez",
                DNICliente = "00123456789",
                MesaId = 1,
                CantidadPersona = 4,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Cliente prefiere mesa cerca de la ventana"
            };

            var added = await repo.AddAsync(reserva);

            // act
            Func<Task> act = async () => await repo.UpdateAsync(added.Id, null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact]
        public async Task GetByIdAsync_Should_Return_Reserva_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            var reserva = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Pérez",
                DNICliente = "00123456789",
                MesaId = 1,
                CantidadPersona = 4,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Cliente prefiere mesa cerca de la ventana"
            };

            var added = await repo.AddAsync(reserva);

            // act
            var act = await repo.GetByIdAsync(added.Id);

            // assert
            act.Should().NotBeNull();
            act!.NombreCliente.Should().Be(reserva.NombreCliente);
            act.Id.Should().Be(added.Id);
            act.DNICliente.Should().Be(reserva.DNICliente);
        }



        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Reserva_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            // act
            var act = await repo.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted_Reserva()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            var reserva = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Pérez",
                DNICliente = "00123456789",
                MesaId = 1,
                CantidadPersona = 4,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Cliente prefiere mesa cerca de la ventana"
            };

            var added = await repo.AddAsync(reserva);

            // act
            var act = await repo.DeleteAsync(added.Id);

            // assert
            act.Should().BeTrue();
        }



        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Deleted_Reserva()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            // act
            var act = await repo.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Reservas_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            var reserva1 = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Juan Pérez",
                DNICliente = "00123456789",
                MesaId = 1,
                CantidadPersona = 4,
                Estado = EstadoReserva.Activa,
                FechaReserva = DateTime.Now.AddDays(1),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Mesa cerca de la ventana"
            };

            var reserva2 = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "María Gómez",
                DNICliente = "00987654321",
                MesaId = 2,
                CantidadPersona = 2,
                Estado = EstadoReserva.EnProceso,
                FechaReserva = DateTime.Now.AddDays(2),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = null
            };

            var reserva3 = new Reserva()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                NombreCliente = "Carlos Rodríguez",
                DNICliente = "00112233445",
                MesaId = 3,
                CantidadPersona = 6,
                Estado = EstadoReserva.Cancelada,
                FechaReserva = DateTime.Now.AddDays(3),
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Observaciones = "Celebración de cumpleaños"
            };

            await repo.AddAsync(reserva1);
            await repo.AddAsync(reserva2);
            await repo.AddAsync(reserva3);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().NotBeNullOrEmpty();
            act.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_Reservas_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ReservasRepository(context);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().BeEmpty();
        }


    }
}
