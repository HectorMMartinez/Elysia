using Elysia.Core.Application.Dtos.shift;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IShiftService : IGenericService<Shift,ShiftResponseDto,EditarShiftDto,CreateShiftDto>
    {
        Task<List<ShiftResponseDto>?> GetAllTurnoByPropietarioId(string PropietarioId);
    }
}
