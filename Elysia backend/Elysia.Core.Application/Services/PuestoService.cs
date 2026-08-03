using AutoMapper;
using Elysia.Core.Application.Dtos.puesto;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class PuestoService : GenericService<Puesto, PuestoResponseDto, EditarPuestoDto, CreatePuestoDto>,IPuestoService
    {
        public PuestoService(IGenericRepository<Puesto> genericRepository, IMapper _mapper) : base(genericRepository, _mapper)
        {
        }
    }
}
