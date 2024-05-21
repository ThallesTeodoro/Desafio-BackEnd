using MediatR;

namespace DesafioBackEnd.Application.Plan.List;

public record ListPlanQuery : IRequest<List<PlanResponse>>;
