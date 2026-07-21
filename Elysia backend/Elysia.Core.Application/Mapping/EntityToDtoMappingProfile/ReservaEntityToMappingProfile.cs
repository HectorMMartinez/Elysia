using AutoMapper;
using Elysia.Core.Application.Dtos.reservas;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class ReservaEntityToMappingProfile : Profile
    {
        public ReservaEntityToMappingProfile()
        {
        
            CreateMap<EditarReservaDto,EditarReservaRequestDto>()
                .ReverseMap()
                .ForMember(x => x.FechaActualizacion, opt => opt.Ignore())
                .ForMember(x => x.FechaCreacion, opt => opt.Ignore())
                .ForMember(x => x.IdPropietario, opt => opt.Ignore());


            CreateMap<CreateReservaDto, CreateReservaRequestDto>()
                     .ReverseMap()
                      .ForMember(x => x.FechaActualizacion, opt => opt.Ignore())
                      .ForMember(x => x.FechaCreacion, opt => opt.Ignore())
                      .ForMember(x => x.IdPropietario, opt => opt.Ignore());

            CreateMap<Reserva, ReservaResponseDto>().ReverseMap();
            CreateMap<CreateReservaDto,Reserva>().ReverseMap();
            CreateMap<EditarReservaDto, Reserva>().ReverseMap();
               







        }
    }
}
