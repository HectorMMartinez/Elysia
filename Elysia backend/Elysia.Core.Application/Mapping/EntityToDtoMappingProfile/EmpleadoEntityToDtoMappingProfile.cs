using AutoMapper;
using Elysia.Core.Application.Dtos.empleado;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class EmpleadoEntityToDtoMappingProfile : Profile
    {
        public EmpleadoEntityToDtoMappingProfile()
        {
            CreateMap<Empleado, EmpleadoResponseDto>().ReverseMap();
            CreateMap<Empleado, CreateEmpleadoDto>().ReverseMap();
            CreateMap<Empleado, EditarEmpleadoDto>().ReverseMap();
            CreateMap<EditarEmpleadoRequestDto,EditarEmpleadoDto>().ReverseMap();
            CreateMap<CreateEmpleadoRequestDto, CreateEmpleadoDto>().ReverseMap();
            
            
        
        
        }


    }
}
