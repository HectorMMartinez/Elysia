using AutoMapper;
using Elysia.Core.Application.Dtos.menu;
using Elysia.Core.Application.Interfaces;
using Elysia.Core.Domain.Entities;
using Elysia.Core.Domain.interfaces;
using ReservaBook.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Services
{
    public class MenuServices : GenericService<Menu, MenuResponseDto, EditarMenuDto, CreateMenuDto>, IMenuService
    {
        private readonly IMenuRepository repo;

        public MenuServices(IMenuRepository repo, IMapper _mapper) : base(repo, _mapper)
        {
            this.repo = repo;
        }



    }
}
