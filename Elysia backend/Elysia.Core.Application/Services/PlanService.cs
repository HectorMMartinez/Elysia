using AutoMapper;
using Elysia.Core.Application.Dtos.plan;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System;

namespace Elysia.Core.Application.Services
{
    public class PlanService : GenericService<Plan, PlanResponseDto, EditPlanDto, SavePlanDto>, IPlanService
    {
        public PlanService(IGenericRepository<Plan> genericRepository, IMapper _mapper) : base(genericRepository, _mapper)
        {
        }
    }
}
