using AutoMapper;
using Elysia.Core.Application.Dtos.Tarjeta;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class TarjetaEntityToDtos : Profile
    {

        public TarjetaEntityToDtos()
        {
            CreateMap<Tarjeta, SaveTarjetaDto>()
                 .ReverseMap();


            CreateMap<Tarjeta,EditTarjetaDto>()
                .ReverseMap();

            CreateMap<Tarjeta, TarjetaResponseDto>()
                .ReverseMap();
           
        
        
        }


    }
}
