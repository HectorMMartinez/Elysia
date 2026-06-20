using Elysia.Core.Application.Dtos.membresia;
using Elysia.Core.Domain.Common;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IMembresiaService : IGenericService<Membresia,MembresiaResponseDto,EditMembresiaDto,SaveMembresiaDto>
    {

        Task<bool> CambiarEstadoAsync(int id,MembresiaEstado estado);   

    }
}
