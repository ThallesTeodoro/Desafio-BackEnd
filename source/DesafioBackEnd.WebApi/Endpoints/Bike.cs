using Carter;
using DesafioBackEnd.Application.Bikes.Create;
using DesafioBackEnd.Application.Bikes.List;
using DesafioBackEnd.Application.Bikes.Update;
using DesafioBackEnd.Application.Common;
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
        app.MapGet("/", ListBikes)
            .RequireAuthorization(PermissionEnum.ManageBikes)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<PaginationResponse<BikeResponse>, object>>(StatusCodes.Status201Created)
            .WithOpenApi();

        app.MapPost("/", CreateBike)
            .RequireAuthorization(PermissionEnum.ManageBikes)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status201Created)
            .WithOpenApi();

        app.MapPatch("/{bikeId}", UpdateBike)
            .RequireAuthorization(PermissionEnum.ManageBikes)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<BikeResponse, object>>(StatusCodes.Status200OK)
            .WithOpenApi();
    }

    private async Task<IResult> ListBikes(
        [AsParameters] ListBikeRequest request,
        ISender sender)
    {
        var result = await sender.Send(new ListBikeQuery(request.Page, request.PageSize, request.Plate));
        var response = new JsonResponse<PaginationResponse<BikeResponse>, object>(StatusCodes.Status200OK, result, null);

        return Results.Ok(response);
    }

    private async Task<IResult> CreateBike(
        [FromBody] CreateBikeRequest request,
        ISender sender)
    {
        await sender.Send(new CreateBikeCommand(
            request.year,
            request.type,
            request.plate
        ));

        return Results.Created();
    }

    private async Task<IResult> UpdateBike(
        Guid bikeId,
        [FromBody] UpdateBikeRequest request,
        ISender sender)
    {
        var bike = await sender.Send(new UpdateBikeCommand(bikeId, request.Plate));

        var response = new JsonResponse<BikeResponse, object>(StatusCodes.Status200OK, bike, null);

        return Results.Ok(response);
    }
}
