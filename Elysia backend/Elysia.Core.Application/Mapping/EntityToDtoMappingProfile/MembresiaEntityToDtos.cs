using AutoMapper;
using Elysia.Core.Application.Dtos.membresia;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class MembresiaEntityToDtos : Profile
    {
        public MembresiaEntityToDtos()
        {

            CreateMap<Membresia, SaveMembresiaDto>()
                .ReverseMap();

            CreateMap<Membresia,MembresiaResponseDto>()
                .ReverseMap();

            CreateMap<Membresia,EditMembresiaDto>() 
                .ReverseMap();
        
        
        }

    }
}
