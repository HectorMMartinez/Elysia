using Elysia.Core.Application.Dtos.plato;
using Elysia.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Interfaces
{
    public interface IPlatoService : IGenericService<Plato,PlatoResponseDto,EditarPlatoDto, CreatePlatoDto>
    {
        Task<List<MostrarPlatoConIngredientesDto?>> GetlAllConIngredientesAsync(string propietarioId);
        Task<MostrarPlatoConIngredientesDto?> GetByIdConIgrendientesAsync(int id);
    }
}
