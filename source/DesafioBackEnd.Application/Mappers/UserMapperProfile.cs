using AutoMapper;
using DesafioBackEnd.Application.User.GetUser;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Application.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<Domain.Entities.User, UserResponse>()
            .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.UserDetail, opt => opt.MapFrom(src => src.DeliveryDetail));

        CreateMap<DeliveryDetail, UserDetailResponse>();
    }
}
