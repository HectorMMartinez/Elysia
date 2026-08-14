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
    public class PedidoRepositoryTest
    {
        private readonly DbContextOptions<ElysiaContext> _dbContextOption;


        public PedidoRepositoryTest()
        {

            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"pedidoDbMemory_{Guid.NewGuid()}").Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Pedido_To_Database()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            var pedido = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 1,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
                Total = 850.00m
            };

            // act
            var act = await repo.AddAsync(pedido);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.IdMesa.Should().Be(1);
            act.Total.Should().Be(850.00m);
            act.Estado.Should().Be(EstadoPedido.Pendiente);

            var data = context.Pedidos.ToList();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Exception_When_Not_Add_Pedido()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            // act
            Func<Task> act = async () => await repo.AddAsync(null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Pedido()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            var pedido = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 1,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
                Total = 850.00m
            };

            var added = await repo.AddAsync(pedido);

            // modificamos
            added.Estado = EstadoPedido.EnPreparacion;
            added.Total = 920.00m;
            added.FechaActualizacion = DateTime.Now;

            // act
            var act = await repo.UpdateAsync(added.Id, added);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(added.Id);
            act.Estado.Should().Be(EstadoPedido.EnPreparacion);
            act.Total.Should().Be(920.00m);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Exception_When_Not_Updated_Pedido()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            var pedido = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 1,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
                Total = 850.00m
            };

            var added = await repo.AddAsync(pedido);

            // act
            Func<Task> act = async () => await repo.UpdateAsync(added.Id, null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Pedido_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            var pedido = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 1,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
                Total = 850.00m
            };

            var added = await repo.AddAsync(pedido);

            // act
            var act = await repo.GetByIdAsync(added.Id);

            // assert
            act.Should().NotBeNull();
            act!.Id.Should().Be(added.Id);
            act.IdMesa.Should().Be(1);
            act.Total.Should().Be(850.00m);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Pedido_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            // act
            var act = await repo.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted_Pedido()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            var pedido = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 1,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
                Total = 850.00m
            };

            var added = await repo.AddAsync(pedido);

            // act
            var act = await repo.DeleteAsync(added.Id);

            // assert
            act.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Deleted_Pedido()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            // act
            var act = await repo.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Pedidos_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            var pedido1 = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 1,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
                Total = 850.00m
            };

            var pedido2 = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 2,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.EnPreparacion,
                Total = 1200.00m
            };

            var pedido3 = new Pedido()
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                IdMesa = 3,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Estado = EstadoPedido.Finalizado,
                Total = 650.50m
            };

            await repo.AddAsync(pedido1);
            await repo.AddAsync(pedido2);
            await repo.AddAsync(pedido3);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().NotBeNullOrEmpty();
            act.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_Pedidos_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PedidoRepository(context);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().BeEmpty();
        }

    }
}
