using AutoMapper;
using Elysia.Core.Application.Dtos.movimientoInventario;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class MovimientoInventariotToDtoMappingProfile : Profile
    {
        public MovimientoInventariotToDtoMappingProfile()
        {
            CreateMap<MovimientoInventario, CrearMovimientoInventarioResponseDto>()
                .ReverseMap();

            CreateMap<CrearMovimientoInventarioDto,CrearMovimientoInvetarioRequestDto>()
                .ReverseMap();
        }
    }
}
