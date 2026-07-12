

using AutoMapper;
using Elysia.Core.Application.Interfaces;
using ReservaBook.Core.Domain.Interfaces;

namespace Elysia.Core.Application.Services
{
    public class GenericService<Tentity,Tresponse,TEditDto, TAddDto> : IGenericService<Tentity,Tresponse,TEditDto, TAddDto> 
        where Tentity : class where Tresponse : class where TAddDto : class where TEditDto : class
    {

        private readonly IGenericRepository<Tentity> _genericRepo;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Tentity> genericRepository, IMapper _mapper)
        {

            this._mapper = _mapper;
            _genericRepo = genericRepository;

        }



        public virtual async Task<Tresponse?> AddAsync(TAddDto? entity)
        {

            if (entity == null)
            {

                return null;

            }

            var newEntity = _mapper.Map<Tentity>(entity);


            var returnEntity = await _genericRepo.AddAsync(newEntity);


            var response = _mapper.Map<Tresponse>(returnEntity);
            return response;

        }

      

        public virtual async Task<bool> DeleteAsync(int id)
        {


            if (id <= 0)
            {
                return false;
            }


            return await _genericRepo.DeleteAsync(id);

        }





        public virtual async Task<Tentity?> GetByIdAsync(int id)
        {

            if (id <= 0)
            {
                return null;
            }

            var entity = await _genericRepo.GetByIdAsync(id);
            return _mapper.Map<Tentity>(entity);

        }




        public virtual async Task<List<Tentity?>> GetlAllAsync()
        {


            var listEntity = await _genericRepo.GetlAllAsync();

            return _mapper.Map<List<Tentity>>(listEntity)!;


        }





        public virtual async Task<Tresponse?> UpdateAsync(int id, TEditDto? entity)
        {

            if (id <= 0)
            {
                return null;

            }


            var UpdatedEntity = _mapper.Map<Tentity>(entity);
            var entityUpdated = await _genericRepo.UpdateAsync(id, UpdatedEntity);


            var dto = _mapper.Map<Tresponse>(entityUpdated);
            return dto;

        }

    }
}
