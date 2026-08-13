using AutoMapper;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Application.Mapping.EntityToDtoMappingProfile;
using Elysia.Core.Application.Services;
using Elysia.Core.Domain.Entities;
using Elysia.Infraestructure.persistences.Contexts;
using Elysia.Infraestructure.persistences.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.unit.Tests.Services
{
    public class ProductoServiceTest
    {

        private readonly IMapper mapper;
        private readonly DbContextOptions<ElysiaContext> _dbContextOptions;




        public ProductoServiceTest()
        {

            _dbContextOptions = new DbContextOptionsBuilder<ElysiaContext>()
                                    .UseInMemoryDatabase(databaseName: $"productoServiceDbInMemory_{Guid.NewGuid()}")
                                    .Options;


            var loggerFactory = LoggerFactory.Create(cfg =>
            {


                cfg.AddConsole();

            });


            var config = new MapperConfiguration(opt =>
            {

                opt.AddProfile<ProductoEntityToDtosMappingProfile>();



            }, loggerFactory);

            mapper = config.CreateMapper();

        }



        #region private method
        public productoService CreateService()
        {

            var context = new ElysiaContext(_dbContextOptions);
            var repo = new ProductoRepository(context);

            var service = new productoService(repo, mapper);
            return service;

        }
        #endregion



        [Fact]
        public async Task AddAsync_should_return_add_dto()
        {

            //arrange
            var service = CreateService();

            var producto = new CreateProductoDto()
            {
                Nombre = "Aguacate",
                Descripcion = "Es un vegetal alimenticio...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 250,
                StockMinimo = 100,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };


            //act
            var act = await service.AddAsync(producto);



            //assert
            act.Should().NotBeNull();
            act.Nombre.Should().Be(producto.Nombre);
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
        public async Task UpdateAsync_should_return_new_Product()
        {

            //arrange
            var service = CreateService();

            var producto = new CreateProductoDto()
            {
                Nombre = "Aguacate",
                Descripcion = "Es un vegetal alimenticio...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 250,
                StockMinimo = 100,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };




            //act
            var add = await service.AddAsync(producto);
            var map = mapper.Map<EditarProductoDto>(add);
            map.Nombre = "Limon";
            var act = await service.UpdateAsync(add.Id,map);



            //assert
            act.Should().NotBeNull();
            act.Nombre.Should().Be(map.Nombre);
            act.Id.Should().BeGreaterThan(0);
        }




        [Fact]
        public async Task UpdateAsync_should_return_response_when_not_add()
        {

            //arrange
            var service = CreateService();

            var producto = new CreateProductoDto()
            {
                Nombre = "Aguacate",
                Descripcion = "Es un vegetal alimenticio...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 250,
                StockMinimo = 100,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };




            //act
            var add = await service.AddAsync(producto);
            var map = mapper.Map<EditarProductoDto>(add);
           
            var act = await service.UpdateAsync(add.Id, null);

            //assert
            act.Should().NotBeNull();
            act.HasError.Should().BeTrue();
        }



        [Fact]
        public async Task GetByIdAsync_should_return_producto_when_exist()
        {

            //arrange
            var service = CreateService();

            var producto = new CreateProductoDto()
            {
                Nombre = "Aguacate",
                Descripcion = "Es un vegetal alimenticio...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 250,
                StockMinimo = 100,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };




            //act
            var add = await service.AddAsync(producto);
            var act = await service.GetByIdAsync(add.Id);



            //assert
            act.Should().NotBeNull();
            act.Nombre.Should().Be(producto.Nombre);
            act.Id.Should().Be(add.Id);
        }





        [Fact]
        public async Task GetByIdAsync_should_return_null_when_product_not_exist()
        {

            //arrange
            var service = CreateService();

          

            //act
            var act = await service.GetByIdAsync(999);



            //assert
            act.Should().BeNull();
            
        }



        [Fact]
        public async Task DeleteIdAsync_should_return_true_when_producto_deleted()
        {

            //arrange
            var service = CreateService();

            var producto = new CreateProductoDto()
            {
                Nombre = "Aguacate",
                Descripcion = "Es un vegetal alimenticio...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 250,
                StockMinimo = 100,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };




            //act
            var add = await service.AddAsync(producto);
            var act = await service.DeleteAsync(add.Id);



            //assert
            act.Should().BeTrue();
          
        }




        [Fact]
        public async Task DeleteIdAsync_should_return_false_when_producto_not_deleted()
        {

            //arrange
            var service = CreateService();

           



            //act
            var act = await service.DeleteAsync(99);



            //assert
            act.Should().BeFalse();

        }





        [Fact]
        public async Task GetAllAsync_should_return_list_when_find_products()
        {

            //arrange
            var service = CreateService();


            var producto1 = new CreateProductoDto()
            {
                Nombre = "Aguacate",
                Descripcion = "Es un vegetal alimenticio...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 250,
                StockMinimo = 100,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };


            var producto2 = new CreateProductoDto()
            {
                Nombre = "Sal",
                Descripcion = "Es un mineral comestico...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 100,
                StockMinimo = 20,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };


            var producto3 = new CreateProductoDto()
            {
                Nombre = "Pollo",
                Descripcion = "Es un alimento alto en proteina...",
                Activo = true,
                FechaCreacion = DateTime.Now,
                IdPropietario = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                UnidadMedida = "Lib",
                StockActual = 300,
                StockMinimo = 100,
                Imagen = "https://upload.wikimedia.org/wikipedia/commons/a/a2/Avocado_fruit_persea_americana.jpg"

            };


            var list = new List<CreateProductoDto>();
            list.Add(producto1);
            list.Add(producto2);
            list.Add(producto3);

            foreach (var item in list)
            {
                await service.AddAsync(item);
            }




            //act
            var act = await service.GetlAllAsync();



            //assert
            act.Should().HaveCountGreaterThan(2);
            act.Should().NotBeEmpty();
          
        }



        [Fact]
        public async Task GetAllAsync_should_return_empty_when_producto_not_founds()
        {

            //arrange
            var service = CreateService();


          


            //act
            var act = await service.GetlAllAsync();



            //assert
            act.Should().BeEmpty();

        }




    }
}
