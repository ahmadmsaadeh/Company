using AutoMapper;
using Company.DTOs;
using Company.Models;

namespace Company.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();
        }
    }
}