using AutoMapper;
using MyApi.Models;
using MyApi.DTOs;

namespace MyApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterRequest, RegisterRequestDto>().ReverseMap();
        CreateMap<LoginRequest, LoginRequestDto>().ReverseMap();
    }
}
