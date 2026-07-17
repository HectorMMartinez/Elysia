using Elysia.Core.Application.Dtos.Mesa;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IMesaService : IGenericService<Mesa,MesaResponseDto,EditarMesaDto,CreateMesaDto>
    {
        Task<List<Mesa?>> GetAllByPropietarioIdAsync(string propietarioId);

    }

}
