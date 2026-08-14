using Elysia.Core.Application.Dtos.puesto;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IPuestoService : IGenericService<Puesto,PuestoResponseDto,EditarPuestoDto,CreatePuestoDto>
    {

    }
}
