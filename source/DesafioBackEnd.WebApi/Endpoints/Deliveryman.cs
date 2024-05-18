using Carter;
using DesafioBackEnd.Application.Deliveryman.Register;
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
            .DisableAntiforgery();
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
}
