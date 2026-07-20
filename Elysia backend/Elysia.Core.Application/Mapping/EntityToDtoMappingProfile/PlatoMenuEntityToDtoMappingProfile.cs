using AutoMapper;
using Elysia.Core.Application.Dtos.platoMenu;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class PlatoMenuEntityToDtoMappingProfile : Profile
    {
        public PlatoMenuEntityToDtoMappingProfile()
        {
            CreateMap<PlatoMenu, CreatePlatoMenuDataDto>()
                     .ReverseMap()
                     .ForMember(x => x.Id, opt => opt.Ignore());


            CreateMap<PlatoMenu, CreatePlatoMenuDto>().ReverseMap();
        }

    }
}
