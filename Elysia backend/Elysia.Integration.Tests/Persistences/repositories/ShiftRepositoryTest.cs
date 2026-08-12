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
    public class ShiftRepositoryTest
    {

        private readonly DbContextOptions<ElysiaContext> _dbContextOption;


        public ShiftRepositoryTest()
        {

            _dbContextOption = new DbContextOptionsBuilder<ElysiaContext>()
                .UseInMemoryDatabase(databaseName: $"shiftDbMemory_{Guid.NewGuid()}").Options;

        }



        [Fact]
        public async Task AddAsync_Should_Handle_Midnight_StartTime()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ShiftRepository(context);

            var shift = new Shift()
            {
                Name = "Turno Medianoche",
                StartTime = new TimeOnly(0, 0),          // 00:00
                EndTime = new TimeOnly(8, 0),
                PropietarioId = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            };

            // act
            var act = await repo.AddAsync(shift);

            // assert
            act.Should().NotBeNull();
            act.StartTime.Should().Be(new TimeOnly(0, 0));
            act.EndTime.Should().Be(new TimeOnly(8, 0));
        }

        [Fact]
        public async Task AddAsync_Should_Handle_EndOfDay_EndTime()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ShiftRepository(context);

            var shift = new Shift()
            {
                Name = "Turno Cierre",
                StartTime = new TimeOnly(16, 0),
                EndTime = new TimeOnly(23, 59),          // 23:59
                PropietarioId = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            };

            // act
            var act = await repo.AddAsync(shift);

            // assert
            act.Should().NotBeNull();
            act.StartTime.Should().Be(new TimeOnly(16, 0));
            act.EndTime.Should().Be(new TimeOnly(23, 59));
        }

        [Fact]
        public async Task AddAsync_Should_Handle_Overnight_Shift()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ShiftRepository(context);

            var shift = new Shift()
            {
                Name = "Turno Nocturno",
                StartTime = new TimeOnly(22, 0),         // 22:00
                EndTime = new TimeOnly(6, 0),            // 06:00 del día siguiente
                PropietarioId = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            };

            // act
            var act = await repo.AddAsync(shift);

            // assert
            act.Should().NotBeNull();
            act.StartTime.Should().Be(new TimeOnly(22, 0));
            act.EndTime.Should().Be(new TimeOnly(6, 0));
            // Nota: StartTime > EndTime es válido en turnos nocturnos
            act.StartTime.Should().BeAfter(act.EndTime);
        }

        [Fact]
        public async Task AddAsync_Should_Handle_Same_Start_And_End_Time()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ShiftRepository(context);

            var shift = new Shift()
            {
                Name = "Turno Especial",
                StartTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(12, 0),           // misma hora
                PropietarioId = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            };

            // act
            var act = await repo.AddAsync(shift);

            // assert
            act.Should().NotBeNull();
            act.StartTime.Should().Be(act.EndTime);
        }

        [Fact]
        public async Task AddAsync_Should_Handle_Max_TimeOnly_Values()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ShiftRepository(context);

            var shift = new Shift()
            {
                Name = "Turno Extremo",
                StartTime = TimeOnly.MinValue,           // 00:00:00.000
                EndTime = TimeOnly.MaxValue,             // 23:59:59.9999999
                PropietarioId = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            };

            // act
            var act = await repo.AddAsync(shift);

            // assert
            act.Should().NotBeNull();
            act.StartTime.Should().Be(TimeOnly.MinValue);
            act.EndTime.Should().Be(TimeOnly.MaxValue);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Overnight_Shift_Correctly()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ShiftRepository(context);

            var shift = new Shift()
            {
                Name = "Turno Noche",
                StartTime = new TimeOnly(22, 0),
                EndTime = new TimeOnly(6, 0),
                PropietarioId = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            };

            var added = await repo.AddAsync(shift);

            // modificamos a otro turno nocturno
            added.StartTime = new TimeOnly(23, 30);
            added.EndTime = new TimeOnly(7, 30);

            // act
            var act = await repo.UpdateAsync(added.Id, added);

            // assert
            act.Should().NotBeNull();
            act.StartTime.Should().Be(new TimeOnly(23, 30));
            act.EndTime.Should().Be(new TimeOnly(7, 30));
            act.StartTime.Should().BeAfter(act.EndTime);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Preserve_TimeOnly_Precision()
        {
            // arrange
            var context = new ElysiaContext(_dbContextOption);
            var repo = new ShiftRepository(context);

            var shift = new Shift()
            {
                Name = "Turno Preciso",
                StartTime = new TimeOnly(9, 15, 30),     // con segundos
                EndTime = new TimeOnly(17, 45, 10),
                PropietarioId = "93789a16-d271-48b0-a1a0-bd2a6074052e"
            };

            var added = await repo.AddAsync(shift);

            // act
            var act = await repo.GetByIdAsync(added.Id);

            // assert
            act.Should().NotBeNull();
            act!.StartTime.Should().Be(new TimeOnly(9, 15, 30));
            act.EndTime.Should().Be(new TimeOnly(17, 45, 10));
        }


    }
}
