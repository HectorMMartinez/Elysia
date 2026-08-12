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
    public class EmpleadoRepositoryTest
    {
        private readonly DbContextOptions<ElysiaContext> _dbContextOption;

        public EmpleadoRepositoryTest()
        {
            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"empleadoDbMemory_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Empleado_To_Database()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            var empleado = new Empleado()
            {
                FirstName = "Carlos",
                LastName = "Ramírez",
                Email = "carlos.ramirez@elysia.com",
                Phone = "809-555-1234",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
                Salary = 25000.00m,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 1
            };

            // act
            var act = await repo.AddAsync(empleado);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.FirstName.Should().Be(empleado.FirstName);
            act.LastName.Should().Be(empleado.LastName);
            act.Email.Should().Be(empleado.Email);
            act.IsActive.Should().BeTrue();

            var data = context.Empleados.ToList();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Exception_When_Not_Add_Empleado()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            // act
            Func<Task> act = async () => await repo.AddAsync(null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Empleado()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            var empleado = new Empleado()
            {
                FirstName = "Carlos",
                LastName = "Ramírez",
                Email = "carlos.ramirez@elysia.com",
                Phone = "809-555-1234",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
                Salary = 25000.00m,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 1
            };

            var added = await repo.AddAsync(empleado);

            // modificamos
            added.FirstName = "Carlos Alberto";
            added.Salary = 28000.00m;
            added.Phone = "809-555-9999";

            // act
            var act = await repo.UpdateAsync(added.Id, added);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(added.Id);
            act.FirstName.Should().Be("Carlos Alberto");
            act.Salary.Should().Be(28000.00m);
            act.Phone.Should().Be("809-555-9999");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Exception_When_Not_Updated_Empleado()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            var empleado = new Empleado()
            {
                FirstName = "Carlos",
                LastName = "Ramírez",
                Email = "carlos.ramirez@elysia.com",
                Phone = "809-555-1234",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
                Salary = 25000.00m,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 1
            };

            var added = await repo.AddAsync(empleado);

            // act
            Func<Task> act = async () => await repo.UpdateAsync(added.Id, null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Empleado_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            var empleado = new Empleado()
            {
                FirstName = "Carlos",
                LastName = "Ramírez",
                Email = "carlos.ramirez@elysia.com",
                Phone = "809-555-1234",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
                Salary = 25000.00m,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 1
            };

            var added = await repo.AddAsync(empleado);

            // act
            var act = await repo.GetByIdAsync(added.Id);

            // assert
            act.Should().NotBeNull();
            act!.Id.Should().Be(added.Id);
            act.FirstName.Should().Be(empleado.FirstName);
            act.LastName.Should().Be(empleado.LastName);
            act.Email.Should().Be(empleado.Email);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Empleado_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            // act
            var act = await repo.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted_Empleado()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            var empleado = new Empleado()
            {
                FirstName = "Carlos",
                LastName = "Ramírez",
                Email = "carlos.ramirez@elysia.com",
                Phone = "809-555-1234",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
                Salary = 25000.00m,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 1
            };

            var added = await repo.AddAsync(empleado);

            // act
            var act = await repo.DeleteAsync(added.Id);

            // assert
            act.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Deleted_Empleado()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            // act
            var act = await repo.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Empleados_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            var empleado1 = new Empleado()
            {
                FirstName = "Carlos",
                LastName = "Ramírez",
                Email = "carlos.ramirez@elysia.com",
                Phone = "809-555-1234",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
                Salary = 25000.00m,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 1
            };

            var empleado2 = new Empleado()
            {
                FirstName = "Ana",
                LastName = "Martínez",
                Email = "ana.martinez@elysia.com",
                Phone = "809-555-5678",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)),
                Salary = 22000.00m,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 2
            };

            var empleado3 = new Empleado()
            {
                FirstName = "Luis",
                LastName = "Gómez",
                Email = "luis.gomez@elysia.com",
                Phone = "809-555-9012",
                HireDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)),
                Salary = 30000.00m,
                IsActive = false,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = 3
            };

            await repo.AddAsync(empleado1);
            await repo.AddAsync(empleado2);
            await repo.AddAsync(empleado3);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().NotBeNullOrEmpty();
            act.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_Empleados_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new EmpleadoRepository(context);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().BeEmpty();
        }

    }
}
