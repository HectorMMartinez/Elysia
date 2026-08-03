using AutoMapper;
using Elysia.Core.Application.Dtos.empleado;
using Elysia.Core.Application.Dtos.shift;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class ShiftEntityToDtoMappingProfile : Profile
    {


        public ShiftEntityToDtoMappingProfile()
        {

            CreateMap<EditarEmpleadoDto, EditarEmpleadoRequestDto>().ReverseMap();
            CreateMap<CreateEmpleadoDto, CreateEmpleadoRequestDto>().ReverseMap();
            CreateMap<ShiftResponseDto, Shift>();
      
        }

    }
}
