using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.RequestHelpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, Product>();
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}