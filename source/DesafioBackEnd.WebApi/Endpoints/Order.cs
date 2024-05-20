using Carter;
using DesafioBackEnd.Application.Order.RegisterOrder;
using DesafioBackEnd.Domain.Enums;
using DesafioBackEnd.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackEnd.WebApi.Endpoints;

public class Order : CarterModule
{
    public Order()
        : base("/order")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", RegisterOrder)
            .RequireAuthorization(PermissionEnum.RegisterOrder)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<OrderResponse, object>>(StatusCodes.Status201Created)
            .WithOpenApi();
    }

    private async Task<IResult> RegisterOrder(
        [FromBody] RegisterOrderRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var result = await sender.Send(new RegisterOrderCommand(request.Value));

        var response = new JsonResponse<OrderResponse, object>(StatusCodes.Status201Created, result, null);

        httpContext.Response.StatusCode = response.StatusCode;

        return Results.Ok(response);
    }
}
