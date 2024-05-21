using AutoMapper;
using DesafioBackEnd.Application.Bikes.List;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Application.Mappers;

public class BikeMapperProfile : Profile
{
    public BikeMapperProfile()
    {
        CreateMap<Bike, BikeResponse>();
    }
}
