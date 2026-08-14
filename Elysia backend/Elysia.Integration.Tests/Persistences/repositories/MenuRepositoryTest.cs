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
    public class MenuRepositoryTest
    {

        private readonly DbContextOptions<ElysiaContext> _dbContextOption;


        public MenuRepositoryTest()
        {

            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"menuDbMemory_{Guid.NewGuid()}").Options;

        }



        [Fact]
        public async Task AddAsync_Should_Add_Menu_To_Database()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            var menu = new Menu()
            {
                Nombre = "Menú Principal",
                Descripcion = "Menú principal del restaurante",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            // act
            var act = await repo.AddAsync(menu);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.Nombre.Should().Be(menu.Nombre);
            act.Descripcion.Should().Be(menu.Descripcion);
            act.IsPrincipal.Should().BeTrue();

            var data = context.Menus.ToList();
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddAsync_Should_Return_Exception_When_Not_Add_Menu()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            // act
            Func<Task> act = async () => await repo.AddAsync(null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Updated_Menu()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            var menu = new Menu()
            {
                Nombre = "Menú Principal",
                Descripcion = "Menú principal del restaurante",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var added = await repo.AddAsync(menu);

            // modificamos
            added.Nombre = "Menú Ejecutivo";
            added.IsPrincipal = false;
            added.FechaActualizacion = DateTime.Now;

            // act
            var act = await repo.UpdateAsync(added.Id, added);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(added.Id);
            act.Nombre.Should().Be("Menú Ejecutivo");
            act.IsPrincipal.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Exception_When_Not_Updated_Menu()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            var menu = new Menu()
            {
                Nombre = "Menú Principal",
                Descripcion = "Menú principal del restaurante",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var added = await repo.AddAsync(menu);

            // act
            Func<Task> act = async () => await repo.UpdateAsync(added.Id, null!);

            // assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Menu_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            var menu = new Menu()
            {
                Nombre = "Menú Principal",
                Descripcion = "Menú principal del restaurante",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var added = await repo.AddAsync(menu);

            // act
            var act = await repo.GetByIdAsync(added.Id);

            // assert
            act.Should().NotBeNull();
            act!.Nombre.Should().Be(menu.Nombre);
            act.Id.Should().Be(added.Id);
            act.IsPrincipal.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Menu_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            // act
            var act = await repo.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted_Menu()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            var menu = new Menu()
            {
                Nombre = "Menú Principal",
                Descripcion = "Menú principal del restaurante",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var added = await repo.AddAsync(menu);

            // act
            var act = await repo.DeleteAsync(added.Id);

            // assert
            act.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Deleted_Menu()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            // act
            var act = await repo.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Menus_When_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            var menu1 = new Menu()
            {
                Nombre = "Menú Principal",
                Descripcion = "Menú principal del restaurante",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var menu2 = new Menu()
            {
                Nombre = "Menú Ejecutivo",
                Descripcion = "Menú para almuerzos ejecutivos",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = false
            };

            var menu3 = new Menu()
            {
                Nombre = "Menú Infantil",
                Descripcion = "Menú especial para niños",
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.NoDisponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = false
            };

            await repo.AddAsync(menu1);
            await repo.AddAsync(menu2);
            await repo.AddAsync(menu3);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().NotBeNullOrEmpty();
            act.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_Menus_Not_Exists()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new MenuRepository(context);

            // act
            var act = await repo.GetlAllAsync();

            // assert
            act.Should().BeEmpty();
        }



    }

}
