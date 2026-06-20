using Elysia.Core.Application.Dtos.plan;
using Elysia.Core.Domain.Entities;


namespace Elysia.Core.Application.Interfaces
{
    public interface IPlanService : IGenericService<Plan,PlanResponseDto,EditPlanDto,SavePlanDto>
    {

    }
}
