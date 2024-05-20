using Carter;
using DesafioBackEnd.Application.Order.ListDeliverymen;
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

        app.MapGet("/list-notified-deliverymen/{orderId}", GetNotifiedUsers)
            .RequireAuthorization(PermissionEnum.RegisterOrder)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<ListDeliverymanOrderResponse, object>>(StatusCodes.Status200OK)
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

    private async Task<IResult> GetNotifiedUsers(
        Guid orderId,
        ISender sender)
    {
        var result = await sender.Send(new ListDeliverymanCommand(orderId));

        var response = new JsonResponse<ListDeliverymanOrderResponse, object>(StatusCodes.Status200OK, result, null);

        return Results.Ok(response);
    }
}
