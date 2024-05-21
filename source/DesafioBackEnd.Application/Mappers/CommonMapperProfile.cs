using AutoMapper;
using DesafioBackEnd.Application.Common;
using DesafioBackEnd.Domain.Dtos;

namespace DesafioBackEnd.Application.Mappers;

public class CommonMapperProfile : Profile
{
    public CommonMapperProfile()
    {
        CreateMap(typeof(PaginationDto<>), typeof(PaginationResponse<>));
    }
}
