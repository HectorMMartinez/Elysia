using AutoMapper;
using Elysia.Core.Application.Dtos.plato;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class PlatoEntityToDtoMappingProfile : Profile
    {

        public PlatoEntityToDtoMappingProfile()
        {
            CreateMap<CreatePlatoDto, CreatePlatoRequestDto>()
                .ReverseMap()
                .ForMember(x => x.Imagen, opt => opt.Ignore())
                .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                .ForMember(x => x.Fecha, opt => opt.Ignore());
            CreateMap<EditarPlatoDto,EditarPlatoRequestDto>()
                .ReverseMap()
                .ForMember(x => x.Imagen, opt => opt.Ignore())
                .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                .ForMember(x => x.Fecha, opt => opt.Ignore());

            CreateMap<Plato, PlatoResponseDto>()
                .ReverseMap();
            CreateMap<CreatePlatoDto,Plato>().ReverseMap();
            CreateMap<EditarPlatoDto,EditarPlatoRequestDto>().ReverseMap(); 
            CreateMap<EditarPlatoDto,PlatoResponseDto>().ReverseMap();


        }
    }
}
