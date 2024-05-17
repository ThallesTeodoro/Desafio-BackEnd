using Carter;
using DesafioBackEnd.Application.Bikes.Create;
using DesafioBackEnd.Domain.Enums;
using DesafioBackEnd.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackEnd.WebApi.Endpoints;

public class Bike : CarterModule
{
    public Bike()
        : base ("/bike")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", CreateBike)
            .RequireAuthorization(PermissionEnum.ManageBikes)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status201Created)
            .WithOpenApi();
    }

    private async Task<IResult> CreateBike(
        [FromBody] CreateBikeRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(new CreateBikeCommand(
            request.year,
            request.type,
            request.plate
        ));

        return Results.Created();
    }
}
