using AutoMapper;
using Elysia.Core.Application.Dtos.menu;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class MenuEntityToDtoMappginProfile : Profile
    {

        public MenuEntityToDtoMappginProfile()
        {
            CreateMap<CreateMenuDto, CreateMenuRequestDto>()
                .ReverseMap()
                .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                .ForMember(x => x.FechaActualizacion, opt => opt.Ignore())
                .ForMember(x => x.FechaCreacion, opt => opt.Ignore());


            CreateMap<EditarMenuDto, EditarMenuRequestDto>()
                .ReverseMap()
                .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                .ForMember(x => x.FechaCreacion, opt => opt.Ignore())
                .ForMember(x => x.FechaActualizacion, opt => opt.Ignore());


            CreateMap<CreateMenuDto, Menu>().ReverseMap();
            CreateMap<EditarMenuDto, Menu>().ReverseMap();
            CreateMap<Menu,MenuResponseDto>().ReverseMap();
           
            
        }


    }
}
