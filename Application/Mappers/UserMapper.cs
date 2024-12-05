using AutoMapper;
using XMP.Application.DTOs;
using XMP.Domain.Entities;

namespace XMP.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapping for creating new users
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping for updating users
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Mapping to return user data
            CreateMap<User, UserResponseDto>();
            CreateMap<RegisterRequest, RegisterRequestDto>().ReverseMap();
            CreateMap<LoginRequest, LoginRequestDto>().ReverseMap();
        }
    }
}
