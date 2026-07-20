using AutoMapper;
using Elysia.Core.Application.Dtos.plato;
using Elysia.Core.Application.Dtos.platoMenu;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using Microsoft.EntityFrameworkCore;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class PlatoMenuService : GenericService<PlatoMenu, CreatePlatoMenuDataDto, CreatePlatoMenuDataDto, CreatePlatoMenuDataDto>, IPlatoMenuService
    {

        private readonly IMapper _mapper;
        private readonly IPlatoMenuRepository repo;
        private readonly IMenuRepository menuRepository;
        private readonly IPlatoRepository platoRepository;


        public PlatoMenuService(IPlatoRepository platoRepository, IMenuRepository menuRepository, IPlatoMenuRepository repo, IMapper _mapper) : base(repo, _mapper)
        {
            this.repo = repo;
            this._mapper = _mapper;
            this.menuRepository = menuRepository;
            this.platoRepository = platoRepository;
        }




        public async Task<List<CreatePlatoMenuDto?>> AddRangeAsync(CreatePlatoMenuDataDto? list)
        {
            try
            {
                if (list == null)
                {
                    return [];
                }


                if (!list.PlatoIds.Any() || list.PlatoIds.Count == 0)
                {
                    return [];
                }

                var dataList = new List<PlatoMenu>();
                foreach (var Id in list.PlatoIds)
                {
                    var platoMenu = new PlatoMenu() { IdMenu = list.IdMenu, IdPlato = Id };
                    dataList.Add(new PlatoMenu() { IdMenu = platoMenu.IdMenu, IdPlato = platoMenu.IdPlato});
                }

                if (dataList.Count > 0)
                {

                    var data = await repo.AddRangeAsync(dataList);
                    var map = _mapper.Map<List<CreatePlatoMenuDto>>(data);
                    return map;


                }


                return new List<CreatePlatoMenuDto>();

            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al intentar agregar los platos en el menu");
            }
        }









        public async Task<MostrarMenuConPlatosListDto> GetMenusConPlatosAsync(string propietario)
        {
            var response = new MostrarMenuConPlatosListDto()
            {
                HasError = false,
                Errors = [],
                MostrarMenuConPlatosDtos = []
            };

            try
            {
                var menus = await menuRepository
                    .GetAllQuariableAsync()
                    .Where(x => x.IdPropietario == propietario)
                    .Include(x => x.PlatoMenus)
                        .ThenInclude(x => x.Plato)
                    .ToListAsync();

                if (!menus.Any())
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontraron menús registrados para este propietario.");
                    return response;
                }

                foreach (var menu in menus)
                {
                    var dto = new MostrarMenuConPlatosDto()
                    {
                        MenuId = menu.Id,
                        NombreMenu = menu.Nombre,
                        DescripcionMenu = menu.Descripcion,
                        MenuEstado = menu.Estado
                    };

                    foreach (var menuPlato in menu.PlatoMenus)
                    {
                        if (menuPlato.Plato != null)
                            dto.Platos.Add(_mapper.Map<CreatePlatoDto>(menuPlato.Plato));
                    }

                    response.MostrarMenuConPlatosDtos.Add(dto);
                }

                return response;
            }
            catch (Exception)
            {
                throw new Exception("Ocurrió un error al intentar obtener el listado de menús con sus platos.");
            }
        }









    }
}
