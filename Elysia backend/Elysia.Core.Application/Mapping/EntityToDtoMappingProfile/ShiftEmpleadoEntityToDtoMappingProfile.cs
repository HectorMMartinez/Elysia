using AutoMapper;
using Elysia.Core.Application.Dtos.empleado;
using Elysia.Core.Application.Dtos.ShiftEmpleado;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class ShiftEmpleadoEntityToDtoMappingProfile : Profile
    {
        public ShiftEmpleadoEntityToDtoMappingProfile()
        {
        
           CreateMap<CreateShiftEmpleadoDto,CreateShiftEmpleadoRequestDto>().ReverseMap();  
           CreateMap<CreateShiftEmpleadoDto,ShiftEmpleadoResponseDto>().ReverseMap();
            CreateMap<EmployeeShift, CreateShiftEmpleadoDto>().ReverseMap();
            CreateMap<EmployeeShift, ShiftEmpleadoResponseDto>().ReverseMap();
        
        }


    }
}
