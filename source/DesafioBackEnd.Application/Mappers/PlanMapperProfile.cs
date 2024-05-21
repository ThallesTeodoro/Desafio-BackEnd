using AutoMapper;
using DesafioBackEnd.Application.Plan.List;

namespace DesafioBackEnd.Application.Mappers;

public class PlanMapperProfile : Profile
{
    public PlanMapperProfile()
    {
        CreateMap<Domain.Entities.Plan, PlanResponse>();
    }
}
