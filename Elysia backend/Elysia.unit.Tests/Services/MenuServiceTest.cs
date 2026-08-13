using AutoMapper;
using Elysia.Core.Application.Dtos.menu;
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
    public class MenuServiceTest
    {

        private readonly IMapper mapper;
        private readonly DbContextOptions<ElysiaContext> _dbContextOptions;




        public MenuServiceTest()
        {

            _dbContextOptions = new DbContextOptionsBuilder<ElysiaContext>()
                                    .UseInMemoryDatabase(databaseName: $"menuServiceDbInMemory_{Guid.NewGuid()}")
                                    .Options;


            var loggerFactory = LoggerFactory.Create(cfg =>
            {


                cfg.AddConsole();

            });


            var config = new MapperConfiguration(opt =>
            {

                opt.AddProfile<MenuEntityToDtoMappginProfile>();



            }, loggerFactory);

            mapper = config.CreateMapper();

        }



        #region private method
        public MenuServices CreateService()
        {

            var context = new ElysiaContext(_dbContextOptions);
            var repo = new MenuRepository(context);

            var service = new MenuServices(repo, mapper);
            return service;

        }
        #endregion

        [Fact]
        public async Task AddAsync_should_return_add_dto()
        {
            // arrange
            var service = CreateService();

            var menu = new Menu
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            // act
            var act = await service.AddAsync(
                mapper.Map<CreateMenuDto>(menu));

            // assert
            act.Should().NotBeNull();
            act.Nombre.Should().Be(menu.Nombre);
            act.HasError.Should().BeFalse();
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
        public async Task UpdateAsync_should_return_new_Menu()
        {
            // arrange
            var service = CreateService();

            var menu = new CreateMenuDto
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var add = await service.AddAsync(menu);
             

            var edit = mapper.Map<EditarMenuDto>(add);
            edit.IsPrincipal = false;

            // act
            var act = await service.UpdateAsync(add.Id, edit);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(add.Id);
            act.IsPrincipal.Should().BeFalse();
        }



        [Fact]
        public async Task UpdateAsync_should_return_null_when_not_updated()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.UpdateAsync(999, null);

            // assert
            act.Should().BeNull();
            
        }


        [Fact]
        public async Task GetByIdAsync_should_return_menu_when_exist()
        {
            // arrange
            var service = CreateService();

            var menu = new Menu
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var add = await service.AddAsync(
                mapper.Map<CreateMenuDto>(menu));

     
            // act
            var act = await service.GetByIdAsync(add.Id);

            // assert
            act.Should().NotBeNull();
            act.Id.Should().Be(add.Id);
            act.Nombre.Should().Be(add.Nombre); 
        }


        [Fact]
        public async Task GetByIdAsync_should_return_null_when_menu_not_exist()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }



        [Fact]
        public async Task DeleteAsync_should_return_true_when_menu_deleted()
        {
            // arrange
            var service = CreateService();

            var menu = new Menu
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };

            var add = await service.AddAsync(
                mapper.Map<CreateMenuDto>(menu));

            // act
            var act = await service.DeleteAsync(add.Id);

            // assert
            act.Should().BeTrue();
        }




        [Fact]
        public async Task DeleteAsync_should_return_false_when_menu_not_deleted()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }



        [Fact]
        public async Task GetAllAsync_should_return_list_when_find_menus()
        {
            // arrange
            var service = CreateService();

            var menu1 = new Menu
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = true
            };


            var menu2 = new Menu
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.Disponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = false
            };

            var menu3 = new Menu
            {
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                Estado = MenuEstado.NoDisponible,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                IsPrincipal = false
            };

            await service.AddAsync(mapper.Map<CreateMenuDto>(menu1));
            await service.AddAsync(mapper.Map<CreateMenuDto>(menu2));
            await service.AddAsync(mapper.Map<CreateMenuDto>(menu3));

            // act
            var act = await service.GetlAllAsync();

            // assert
            act.Should().NotBeEmpty();
            act.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetAllAsync_should_return_empty_when_menus_not_found()
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
