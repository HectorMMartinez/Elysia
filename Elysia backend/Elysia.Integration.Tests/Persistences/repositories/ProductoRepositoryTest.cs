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
    public class ProductoRepositoryTest
    {

        private readonly DbContextOptions<ElysiaContext> _dbContextOption;


        public ProductoRepositoryTest()
        {

            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"productoDbMemory_{Guid.NewGuid()}").Options;

        }



        [Fact]
        public async Task AddAsync_Should_Add_producto_To_Database()
        {
            //arrange
            var context =  new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

            var producto = new Producto()
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
            var act = await repo.AddAsync(producto);


            //assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.Nombre.Should().Be(producto.Nombre);
            var data =  context.Productos.ToList();
            data.Should().HaveCount(1);

        }




        [Fact]
        public async Task AddAsync_Should_Return_exception_when_Not_Add_Producto()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

            ;


            //act
            Func<Task> act = async () => await repo.AddAsync(null!);


            //assert
            act.Should().ThrowAsync<ArgumentNullException>().WithMessage("value cannot be null. '(parameter enty)'");
        }




        [Fact]
        public async Task UpdateAsync_Should_Return_New_Producto_when_updated_producto()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

            var producto = new Producto()
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
            var addProducto = await repo.AddAsync(producto);
            addProducto.Nombre = "Sal";
            var act = await repo.UpdateAsync(producto.Id,producto);
            

            //assert
            act.Should().NotBeNull();
            act.Id.Should().BeGreaterThan(0);
            act.Nombre.Should().Be(act.Nombre);
          
        }



        [Fact]
        public async Task UpdateAsync_should_Return_exception_when_not_updated_producto()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

            var producto = new Producto()
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
            var addProducto = await repo.AddAsync(producto);
            Func<Task> act = async() => await repo.UpdateAsync(addProducto.Id, null!);


            //assert
            act.Should().ThrowAsync<ArgumentNullException>().WithMessage("value cannot be null. '(parameter enty)'");


        }



        [Fact]
        public async Task GetByAsync_should_Return_Producto_when_producto_exist()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

            var producto = new Producto()
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
            var addProducto = await repo.AddAsync(producto);
            var act = await repo.GetByIdAsync(addProducto.Id);


            //assert
            act.Should().NotBeNull();
            act.Nombre.Should().Be(producto.Nombre);
            act.Id.Should().Be(addProducto.Id);


        }




        [Fact]
        public async Task GetByAsync_should_Return_null_when_producto_not_exist()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

          
            //act
            var act = await repo.GetByIdAsync(999);


            //assert
            act.Should().BeNull();
           
        }




        [Fact]
        public async Task DeleteAsync_should_Return_true_when_deleted_product()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

            var producto = new Producto()
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
            var addProducto = await repo.AddAsync(producto);
            var act = await repo.DeleteAsync(addProducto.Id);


            //assert
            act.Should().BeTrue();
            
        }



        [Fact]
        public async Task DeleteAsync_should_Return_false_when_not_deleted_product()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

          

            //act
            var act = await repo.DeleteAsync(999);


            //assert
            act.Should().BeFalse();

        }



        [Fact]
        public async Task GetAllAsync_should_Return__products_when_exists()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

            var producto1 = new Producto()
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


            var producto2 = new Producto()
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


            var producto3 = new Producto()
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


            var list = new List<Producto>();
            list.Add(producto1);
            list.Add(producto2);
            list.Add(producto3);

            foreach (var item in list)
            {
                await repo.AddAsync(item);
            }


            //act

            var act = await repo.GetlAllAsync();


            //assert
            act.Should().HaveCount(3);
            act.Should().NotBeNullOrEmpty();

        }






        [Fact]
        public async Task GetAllAsync_should_Return_empty_when_products_not_exists()
        {
            //arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ProductoRepository(context);

           
            //act

            var act = await repo.GetlAllAsync();


            //assert
            act.Should().BeEmpty();

        }



    }
}
