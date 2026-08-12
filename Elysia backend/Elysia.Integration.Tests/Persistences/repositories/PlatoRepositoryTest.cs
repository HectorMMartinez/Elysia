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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Elysia.Integration.Tests.Persistences.Repositories
{
    public class PlatoRepositoryTest
    {

        private readonly DbContextOptions<ElysiaContext> _dbContextOption;


        public PlatoRepositoryTest()
        {

            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"platoDbMemory_{Guid.NewGuid()}").Options;

        }


        [Fact]
        public async Task AddAsync_Should_Add_Plato_To_Database()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            var plato = new Plato()
            {
                Nombre = "Ensalada de Aguacate",
                Descripcion = "Ensalada fresca con aguacate, tomate y cebolla",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 250.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            };

            // act
            var act = await repo.AddAsync(plato);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.Nombre.Should().Be(plato.Nombre);
            act.Descripcion.Should().Be(plato.Descripcion);

            var data = context.Platos.ToList();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Exception_When_Not_Add_Plato()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            // act
            Func<Task> act = async () => await repo.AddAsync(null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }



        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Plato()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            var plato = new Plato()
            {
                Nombre = "Ensalada de Aguacate",
                Descripcion = "Ensalada fresca con aguacate",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 250.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            };

            var added = await repo.AddAsync(plato);

            // modificamos
            added.Nombre = "Ensalada César";
            added.Precio = 280.00m;

            // act
            var act = await repo.UpdateAsync(added.Id, added);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(added.Id);
            act.Nombre.Should().Be("Ensalada César");
            act.Precio.Should().Be(280.00m);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Exception_When_Not_Updated_Plato()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            var plato = new Plato()
            {
                Nombre = "Ensalada de Aguacate",
                Descripcion = "Ensalada fresca con aguacate",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 250.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            };


            var added = await repo.AddAsync(plato);

            // act
            Func<Task> act = async () => await repo.UpdateAsync(added.Id, null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Plato_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            var plato = new Plato()
            {
                Nombre = "Ensalada de Aguacate",
                Descripcion = "Ensalada fresca con aguacate",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 250.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            };

            var added = await repo.AddAsync(plato);

            // act
            var act = await repo.GetByIdAsync(added.Id);

            // assert
            act.Should().NotBeNull();
            act!.Nombre.Should().Be(plato.Nombre);
            act.Id.Should().Be(added.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Plato_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            // act
            var act = await repo.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted_Plato()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            var plato = new Plato()
            {
                Nombre = "Ensalada de Aguacate",
                Descripcion = "Ensalada fresca con aguacate",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 250.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            };

            var added = await repo.AddAsync(plato);

            // act
            var act = await repo.DeleteAsync(added.Id);

            // assert
            act.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Deleted_Plato()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            // act
            var act = await repo.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Platos_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            var plato1 = new Plato()
            {
                Nombre = "Ensalada de Aguacate",
                Descripcion = "Ensalada fresca con aguacate",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 250.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-001",
                CategoriaId = 1,
                Estado = PlatoEstado.Disponible
            };

            var plato2 = new Plato()
            {
                Nombre = "Pollo a la Plancha",
                Descripcion = "Pechuga de pollo a la plancha con vegetales",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 320.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-002",
                CategoriaId = 2,
                Estado = PlatoEstado.Disponible
            };

            var plato3 = new Plato()
            {
                Nombre = "Pasta Alfredo",
                Descripcion = "Pasta con salsa alfredo y pollo",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Precio = 280.00m,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Codigo = "PLA-003",
                CategoriaId = 3,
                Estado = PlatoEstado.Disponible
            };

            await repo.AddAsync(plato1);
            await repo.AddAsync(plato2);
            await repo.AddAsync(plato3);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().NotBeNullOrEmpty();
            act.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_Platos_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new PlatoRepository(context);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().BeEmpty();
        }
}
    }


