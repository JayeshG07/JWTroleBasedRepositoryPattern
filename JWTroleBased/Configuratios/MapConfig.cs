using AutoMapper;
using JWTroleBased.Dto;
using JWTroleBased.Models;
using System.Runtime;

namespace JWTroleBased.Configuratios
{
    public class MapConfig:Profile
    {
        public MapConfig()
        {
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
