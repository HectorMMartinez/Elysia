using AutoMapper;
using Elysia.Core.Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elysia.Core.Application.Mapping.EntityToDtoMappingProfile
{
    public class UserDtosMappingProfile : Profile
    {
        public UserDtosMappingProfile() 
        {
            CreateMap<SaveUserRequestDto, RegisterUserRequestDto>()
                .ReverseMap()
                .ForMember(x => x.LogoRestaurante, opt => opt.Ignore())
                .ForMember(x => x.ProfileImage,  opt => opt.Ignore());
                        
                
         
        
        
        
        }
    }
}
