using AutoMapper;
using DesafioBackEnd.Application.Order.RegisterOrder;

namespace DesafioBackEnd.Application.Mappers;

public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<Domain.Entities.Order, OrderResponse>();
    }
}
