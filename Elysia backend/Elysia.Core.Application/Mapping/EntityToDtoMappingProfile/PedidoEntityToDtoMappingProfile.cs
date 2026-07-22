using AutoMapper;
using Elysia.Core.Application.Dtos.pedido;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class PedidoEntityToDtoMappingProfile : Profile
    {
        public PedidoEntityToDtoMappingProfile()
        {
            CreateMap<Pedido, CreatePedidoDto>().ReverseMap();
            CreateMap<DetallesPedido, DetallesPedidoDto>().ReverseMap();
            CreateMap<EditarPedidoDto,EditarPedidoRequestDto>()
                .ReverseMap()
                .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                .ForMember(x => x.FechaActualizacion, opt => opt.Ignore())
                .ForMember(x => x.FechaCreacion, opt => opt.Ignore())
                .ForMember(x => x.Estado, opt => opt.Ignore())
                .ForMember(x => x.DetallesPedidoDtos, opt => opt.MapFrom(src => src.DetallesPedido));


            CreateMap<CreatePedidoDto, CreatePedidoRequestDto>()
                .ReverseMap()
                .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                .ForMember(x => x.Estado, opt => opt.Ignore())
                .ForMember(x => x.FechaActualizacion, opt => opt.Ignore())
                .ForMember(x => x.FechaCreacion, opt => opt.Ignore())
                .ForMember(x => x.DetallesPedidoDtos, opt => opt.MapFrom(src => src.DetallesPedido));


        }
    }
}
