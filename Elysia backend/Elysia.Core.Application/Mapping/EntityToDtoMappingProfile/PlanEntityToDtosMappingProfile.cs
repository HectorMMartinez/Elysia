using AutoMapper;
using Elysia.Core.Application.Dtos.plan;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class PlanEntityToDtosMappingProfile : Profile
    {
        public PlanEntityToDtosMappingProfile()
        {
            CreateMap<Plan, SavePlanDto>()
                .ReverseMap();

            CreateMap<Plan,PlanResponseDto>()
                .ReverseMap();  

            CreateMap<Plan,EditPlanDto>() 
                .ReverseMap();
        
        
        
        }
    }
}
