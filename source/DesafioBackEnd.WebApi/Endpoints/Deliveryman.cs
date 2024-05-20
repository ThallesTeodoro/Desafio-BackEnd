using System.Security.Claims;
using Carter;
using DesafioBackEnd.Application.Deliveryman.Register;
using DesafioBackEnd.Application.Deliveryman.Rent;
using DesafioBackEnd.Application.Deliveryman.ReturnRent;
using DesafioBackEnd.Application.Deliveryman.Update;
using DesafioBackEnd.Domain.Enums;
using DesafioBackEnd.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackEnd.WebApi.Endpoints;

public class Deliveryman : CarterModule
{
    public Deliveryman()
        : base ("/deliveryman")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", RegisterDeliveryman)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<DeliverymanResponse, object>>(StatusCodes.Status201Created)
            .DisableAntiforgery()
            .WithOpenApi();

        app.MapPatch("/{userId}", UpdateDeliverymanCnh)
            .RequireAuthorization(PermissionEnum.DeliverymanRegister)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<DeliverymanResponse, object>>(StatusCodes.Status201Created)
            .DisableAntiforgery()
            .WithOpenApi();

        app.MapPost("/rent-bike", RentBike)
            .RequireAuthorization(PermissionEnum.BikeRent)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<DeliverymanResponse, object>>(StatusCodes.Status201Created)
            .WithOpenApi();

        app.MapPost("/informe-return-date", InformReturnDate)
            .RequireAuthorization(PermissionEnum.BikeRent)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<DeliverymanResponse, object>>(StatusCodes.Status201Created)
            .WithOpenApi();
    }

    private async Task<IResult> RegisterDeliveryman(
        [FromForm] RegisterDeliverymanRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var result = await sender.Send(new RegisterDeliverymanCommand(
            request.Name,
            request.Email,
            request.Cnpj,
            request.Birthdate,
            request.Cnh,
            request.CnhType,
            request.CnhImage
        ));

        var response = new JsonResponse<DeliverymanResponse, object>(StatusCodes.Status201Created, result, null);

        httpContext.Response.StatusCode = response.StatusCode;

        return Results.Ok(response);
    }

    private async Task<IResult> UpdateDeliverymanCnh(
        Guid userId,
        IFormFile cnhImage,
        ISender sender)
    {
        await sender.Send(new UpdateDeliverymanCommand(userId, cnhImage));

        return Results.Ok();
    }

    private async Task<IResult> RentBike(
        [FromBody] RentRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var authUserId = httpContext
            .User
            .Claims
            .Where(c => c.Type == ClaimTypes.NameIdentifier)
            .First()
            .Value;

        await sender.Send(new RentCommand(Guid.Parse(authUserId), request.StartDay, request.EndDay));

        return Results.Created();
    }

    private async Task<IResult> InformReturnDate(
        [FromBody] ReturnRentRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var authUserId = httpContext
            .User
            .Claims
            .Where(c => c.Type == ClaimTypes.NameIdentifier)
            .First()
            .Value;

        var result = await sender.Send(new ReturnRentCommand(Guid.Parse(authUserId), request.PrevEndDay));

        var response = new JsonResponse<ReturnRentResponse, object>(StatusCodes.Status200OK, result, null);

        return Results.Ok(response);
    }
}
