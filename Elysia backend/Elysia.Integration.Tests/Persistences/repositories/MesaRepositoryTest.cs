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
    public class MesaRepositoryTest
    {
        private readonly DbContextOptions<ElysiaContext> _dbContextOption;


        public MesaRepositoryTest()
        {

            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"mesaDbMemory_{Guid.NewGuid()}").Options;

        }

        [Fact]
        public async Task AddAsync_Should_Add_Mesa_To_Database()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            var mesa = new Mesa()
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa cercana a la ventana",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-001"
            };

            // act
            var act = await repo.AddAsync(mesa);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.Nombre.Should().Be(mesa.Nombre);
            act.Capacidad.Should().Be(4);
            act.Estado.Should().Be(MesaEstado.Disponible);

            var data = context.Mesas.ToList();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Exception_When_Not_Add_Mesa()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            // act
            Func<Task> act = async () => await repo.AddAsync(null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Mesa()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            var mesa = new Mesa()
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa cercana a la ventana",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-001"
            };

            var added = await repo.AddAsync(mesa);

            // modificamos
            added.Nombre = "Mesa VIP";
            added.Capacidad = 6;
            added.Estado = MesaEstado.Ocupada;
            added.FechaActualizacion = DateTime.Now;

            // act
            var act = await repo.UpdateAsync(added.Id, added);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(added.Id);
            act.Nombre.Should().Be("Mesa VIP");
            act.Capacidad.Should().Be(6);
            act.Estado.Should().Be(MesaEstado.Ocupada);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Exception_When_Not_Updated_Mesa()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            var mesa = new Mesa()
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa cercana a la ventana",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-001"
            };

            var added = await repo.AddAsync(mesa);

            // act
            Func<Task> act = async () => await repo.UpdateAsync(added.Id, null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Mesa_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            var mesa = new Mesa()
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa cercana a la ventana",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-001"
            };

            var added = await repo.AddAsync(mesa);

            // act
            var act = await repo.GetByIdAsync(added.Id);

            // assert
            act.Should().NotBeNull();
            act!.Nombre.Should().Be(mesa.Nombre);
            act.Id.Should().Be(added.Id);
            act.Capacidad.Should().Be(4);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Mesa_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            // act
            var act = await repo.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted_Mesa()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            var mesa = new Mesa()
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa cercana a la ventana",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-001"
            };

            var added = await repo.AddAsync(mesa);

            // act
            var act = await repo.DeleteAsync(added.Id);

            // assert
            act.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Deleted_Mesa()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            // act
            var act = await repo.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Mesas_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            var mesa1 = new Mesa()
            {
                Nombre = "Mesa 1",
                Descripcion = "Mesa cercana a la ventana",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Disponible,
                Capacidad = 4,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-001"
            };

            var mesa2 = new Mesa()
            {
                Nombre = "Mesa 2",
                Descripcion = "Mesa en el centro del salón",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Ocupada,
                Capacidad = 6,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-002"
            };

            var mesa3 = new Mesa()
            {
                Nombre = "Mesa VIP",
                Descripcion = "Mesa privada",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MesaEstado.Reservada,
                Capacidad = 8,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg",
                Codigo = "MESA-003"
            };

            await repo.AddAsync(mesa1);
            await repo.AddAsync(mesa2);
            await repo.AddAsync(mesa3);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().NotBeNullOrEmpty();
            act.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_Mesas_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MesaRepository(context);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().BeEmpty();
        }

    }
}
