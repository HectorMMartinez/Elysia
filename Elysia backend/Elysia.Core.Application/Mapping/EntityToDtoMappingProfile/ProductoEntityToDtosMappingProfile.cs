using AutoMapper;
using Elysia.Core.Application.Dtos.producto;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class ProductoEntityToDtosMappingProfile : Profile
    {
        public ProductoEntityToDtosMappingProfile()
        {
            CreateMap<Producto, CreateProductoDto>()
                    .ReverseMap();
            CreateMap<EditarProductoDto,Producto>()
                .ReverseMap();
            CreateMap<Producto,ProductoResponseDto>()
                .ReverseMap();
        
            CreateMap<CreateProductoDto,SaveProductoDto>()
                .ReverseMap()
                .ForMember(x => x.IdPropietario, opt => opt.Ignore())
                 .ForMember(x => x.Imagen, opt => opt.Ignore());



            CreateMap<EditarProductoDto, EditarProductoRequestDto>()
                .ReverseMap()
                .ForMember(x => x.Imagen, opt => opt.Ignore());
               
        
        }
    }
}
