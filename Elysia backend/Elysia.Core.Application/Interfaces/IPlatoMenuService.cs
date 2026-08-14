using Elysia.Core.Application.Dtos.platoMenu;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IPlatoMenuService : IGenericService<PlatoMenu,CreatePlatoMenuDataDto, CreatePlatoMenuDataDto, CreatePlatoMenuDataDto>
    {
        
        Task<List<CreatePlatoMenuDto?>> AddRangeAsync(CreatePlatoMenuDataDto? list);
        Task<MostrarMenuConPlatosListDto> GetMenusConPlatosAsync(string propietario);


    }
}
