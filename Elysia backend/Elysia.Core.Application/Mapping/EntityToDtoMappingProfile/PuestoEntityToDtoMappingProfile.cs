using AutoMapper;
using Elysia.Core.Application.Dtos.pedido;
using Elysia.Core.Application.Dtos.puesto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class PuestoEntityToDtoMappingProfile : Profile
    {

        public PuestoEntityToDtoMappingProfile()
        {
            CreateMap<EditarPedidoRequestDto, EditarPuestoDto>().ReverseMap();
            CreateMap<CreatePuestoDto, CreatePuestoRequestDto>().ReverseMap();
            CreateMap<EditarPuestoDto,PuestoResponseDto>().ReverseMap();
            CreateMap<CreatePuestoDto, PuestoResponseDto>().ReverseMap();
           
        
        
        }





    }
}
