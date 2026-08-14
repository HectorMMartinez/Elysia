using AutoMapper;
using Elysia.Core.Application.Dtos.categoriaPlato;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;



namespace Elysia.Core.Application.Services
{
    public class CategoriaPlatoService : GenericService<CategoriaPlato, CategoriaPlatoDto, CategoriaPlatoDto, CategoriaPlatoDto>, ICategoriaPlatoService
    {
        public CategoriaPlatoService(IGenericRepository<CategoriaPlato> genericRepository, IMapper _mapper) : base(genericRepository, _mapper)
        {

        }
    }

}
