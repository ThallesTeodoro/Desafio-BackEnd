using Carter;
using DesafioBackEnd.Application.Plan.List;
using DesafioBackEnd.Domain.Enums;
using DesafioBackEnd.WebApi.Contracts;
using MediatR;

namespace DesafioBackEnd.WebApi.Endpoints;

public class Plan : CarterModule
{
    public Plan()
        : base("/plan")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", ListPlan)
            .RequireAuthorization()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<List<PlanResponse>, object>>(StatusCodes.Status200OK);
    }

    private async Task<IResult> ListPlan(ISender sender)
    {
        var response = new JsonResponse<List<PlanResponse>, object>(
            StatusCodes.Status200OK,
            await sender.Send(new ListPlanQuery()),
            null);

        return Results.Ok(response);
    }
}
