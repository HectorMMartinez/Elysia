using AutoMapper;
using Elysia.Core.Application.Dtos.empleado;
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
using System.Text;
using System.Threading.Tasks;

namespace Elysia.unit.Tests.Services
{
    public class EmpleadoServiceTest
    {

        private readonly IMapper mapper;
        private readonly DbContextOptions<ElysiaContext> _dbContextOptions;
        private ElysiaContext _context = null!;

        public EmpleadoServiceTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"empleadoServiceDbInMemory_{Guid.NewGuid()}")
                .Options;

            _context = new ElysiaContext(_dbContextOptions);

            var loggerFactory = LoggerFactory.Create(cfg =>
            {
                cfg.AddConsole();
            });

            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile<EmpleadoEntityToDtoMappingProfile>();
            }, loggerFactory);

            mapper = config.CreateMapper();
        }

        #region private method

        public EmpleadoService CreateService()
        {
            var empleadoRepository = new EmpleadoRepository(_context);
            var puestoRepository = new PuestoRepository(_context);

            return new EmpleadoService(
                empleadoRepository,
                mapper,
                puestoRepository);
        }

        #endregion


        [Fact]
        public async Task AddAsync_should_return_add_dto()
        {
            // arrange
            var service = CreateService();

            var puestoRepository = new PuestoRepository(_context);

            var puesto = await puestoRepository.AddAsync(new Puesto
            {
                Name = "Mesero",
                Description = "Encargado de atender a los clientes"
             
            });

            var empleado = new CreateEmpleadoDto
            {
                FirstName = "Juan",
                LastName = "Perez",
                Email = "juan.perez@test.com",
                Phone = "8091234567",
                HireDate = DateOnly.FromDateTime(DateTime.Today),
                Salary = 25000,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = puesto.Id
            };

            // act
            var act = await service.AddAsync(empleado);

            // assert
            act.Should().NotBeNull();
          
            act.FirstName.Should().Be(empleado.FirstName);
            act.LastName.Should().Be(empleado.LastName);
            act.Email.Should().Be(empleado.Email);
            act.Phone.Should().Be(empleado.Phone);
            act.Salary.Should().Be(empleado.Salary);
            act.PuestoId.Should().Be(empleado.PuestoId);
            act.RestaurantId.Should().Be(empleado.RestaurantId);
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
        public async Task UpdateAsync_should_return_new_Empleado()
        {
            // arrange
            var service = CreateService();

            var puestoRepository = new PuestoRepository(_context);

            var puesto = await puestoRepository.AddAsync(new Puesto
            {
                Name = "Mesero",
                Description = "Encargado de atender a los clientes"
               
            });

            var empleado = new CreateEmpleadoDto
            {
                FirstName = "Juan",
                LastName = "Perez",
                Email = "juan.perez@test.com",
                Phone = "8091234567",
                HireDate = DateOnly.FromDateTime(DateTime.Today),
                Salary = 25000,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = puesto.Id
            };

            var add = await service.AddAsync(empleado);

            var map = mapper.Map<EditarEmpleadoDto>(add);

            map.FirstName = "Pedro";
            map.LastName = "Martinez";
            map.Salary = 30000;

            // act
            var act = await service.UpdateAsync(add.Id, map);

            // assert
            act.Should().NotBeNull();
            act.FirstName.Should().Be(map.FirstName);
            act.LastName.Should().Be(map.LastName);
            act.Salary.Should().Be(map.Salary);
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
        public async Task GetByIdAsync_should_return_empleado_when_exist()
        {
            // arrange
            var service = CreateService();

            var puestoRepository = new PuestoRepository(_context);

            var puesto = await puestoRepository.AddAsync(new Puesto
            {
                Name = "Mesero",
                Description = "Encargado de atender a los clientes"
              
            });

            var empleado = new CreateEmpleadoDto
            {
                FirstName = "Juan",
                LastName = "Perez",
                Email = "juan.perez@test.com",
                Phone = "8091234567",
                HireDate = DateOnly.FromDateTime(DateTime.Today),
                Salary = 25000,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = puesto.Id
            };

            var add = await service.AddAsync(empleado);

            // act
            var act = await service.GetByIdAsync(add.Id);

            // assert
            act.Should().NotBeNull();
            act.FirstName.Should().Be(empleado.FirstName);
            act.LastName.Should().Be(empleado.LastName);
            act.Email.Should().Be(empleado.Email);
            act.PuestoId.Should().Be(empleado.PuestoId);
            act.Id.Should().Be(add.Id);
        }


        [Fact]
        public async Task GetByIdAsync_should_return_null_when_empleado_not_exist()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.GetByIdAsync(999);

            // assert
            act.Should().BeNull();
        }


        [Fact]
        public async Task DeleteIdAsync_should_return_true_when_empleado_deleted()
        {
            // arrange
            var service = CreateService();

            var puestoRepository = new PuestoRepository(_context);

            var puesto = await puestoRepository.AddAsync(new Puesto
            {
                Name = "Mesero",
                Description = "Encargado de atender a los clientes"
              
            });

            var empleado = new CreateEmpleadoDto
            {
                FirstName = "Juan",
                LastName = "Perez",
                Email = "juan.perez@test.com",
                Phone = "8091234567",
                HireDate = DateOnly.FromDateTime(DateTime.Today),
                Salary = 25000,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = puesto.Id
            };

            var add = await service.AddAsync(empleado);

            // act
            var act = await service.DeleteAsync(add.Id);

            // assert
            act.Should().BeTrue();
        }


        [Fact]
        public async Task DeleteIdAsync_should_return_false_when_empleado_not_deleted()
        {
            // arrange
            var service = CreateService();

            // act
            var act = await service.DeleteAsync(999);

            // assert
            act.Should().BeFalse();
        }


        [Fact]
        public async Task GetAllAsync_should_return_list_when_find_empleados()
        {
            // arrange
            var service = CreateService();

            var puestoRepository = new PuestoRepository(_context);

            var puesto1 = await puestoRepository.AddAsync(new Puesto
            {
                Name = "Mesero",
                Description = "Encargado de atender a los clientes"
              
            });

            var empleado1 = new CreateEmpleadoDto
            {
                FirstName = "Juan",
                LastName = "Perez",
                Email = "juan.perez@test.com",
                Phone = "8091234567",
                HireDate = DateOnly.FromDateTime(DateTime.Today),
                Salary = 25000,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = puesto1.Id
            };

            var add1 = await service.AddAsync(empleado1);


            var puesto2 = await puestoRepository.AddAsync(new Puesto
            {
                Name = "Cocinero",
                Description = "Encargado de preparar los platos"
              
            });

            var empleado2 = new CreateEmpleadoDto
            {
                FirstName = "Pedro",
                LastName = "Martinez",
                Email = "pedro.martinez@test.com",
                Phone = "8091234568",
                HireDate = DateOnly.FromDateTime(DateTime.Today),
                Salary = 30000,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = puesto2.Id
            };

            var add2 = await service.AddAsync(empleado2);


            var puesto3 = await puestoRepository.AddAsync(new Puesto
            {
                Name = "Administrador",
                Description = "Encargado de administrar el restaurante"
              
            });

            var empleado3 = new CreateEmpleadoDto
            {
                FirstName = "Carlos",
                LastName = "Rodriguez",
                Email = "carlos.rodriguez@test.com",
                Phone = "8091234569",
                HireDate = DateOnly.FromDateTime(DateTime.Today),
                Salary = 40000,
                IsActive = true,
                RestaurantId = "93789a16-d271-48b0-a1a0-bd2a6074052e",
                PuestoId = puesto3.Id
            };

            var add3 = await service.AddAsync(empleado3);

            // act
            var act = await service.GetlAllAsync();

            // assert
            act.Should().HaveCount(3);
            act.Should().NotBeEmpty();
        }


        [Fact]
        public async Task GetAllAsync_should_return_empty_when_empleados_not_found()
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
