using AutoMapper;
using DesafioBackEnd.Domain.Contracts.Persistence;
using MediatR;

namespace DesafioBackEnd.Application.Plan.List;

public class ListPlanQueryHandler : IRequestHandler<ListPlanQuery, List<PlanResponse>>
{
    private readonly IPlanRepository _planRepository;
    private readonly IMapper _mapper;

    public ListPlanQueryHandler(IMapper mapper, IPlanRepository planRepository)
    {
        _mapper = mapper;
        _planRepository = planRepository;
    }

    public async Task<List<PlanResponse>> Handle(ListPlanQuery request, CancellationToken cancellationToken)
    {
        var plans = await _planRepository.AllAsync();

        return _mapper.Map<List<PlanResponse>>(plans.OrderBy(p => p.Days));
    }
}
