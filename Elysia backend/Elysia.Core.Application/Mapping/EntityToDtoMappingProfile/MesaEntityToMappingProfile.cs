using AutoMapper;
using Elysia.Core.Application.Dtos.Mesa;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class MesaEntityToMappingProfile : Profile
    {
        public MesaEntityToMappingProfile()
        {
            CreateMap<CreateMesaRequestDto,CreateMesaDto>()
                 .ForMember(x => x.Imagen, opt => opt.Ignore())
                 .ForMember(x => x.Codigo, opt => opt.Ignore())
                 .ForMember(x => x.FechaActualizacion, opt => opt.Ignore())
                 .ForMember(x => x.FechaCreacion, opt => opt.Ignore())
                 .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<EditarMesaRequestDto, EditarMesaDto>()
              .ForMember(x => x.Imagen, opt => opt.Ignore())
              .ForMember(x => x.Codigo, opt => opt.Ignore())
              .ForMember(x => x.FechaActualizacion, opt => opt.Ignore())
              .ForMember(x => x.FechaCreacion, opt => opt.Ignore())
              .ForMember(x => x.IdPropietario, opt => opt.Ignore())
             .ReverseMap();



            CreateMap<Mesa, CreateMesaDto>().ReverseMap();
            CreateMap<Mesa, EditarMesaDto>().ReverseMap();
            CreateMap<Mesa,MesaResponseDto>().ReverseMap();

        } 


    }
}
