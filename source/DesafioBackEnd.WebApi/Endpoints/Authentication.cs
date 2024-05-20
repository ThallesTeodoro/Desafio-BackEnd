using Carter;
using DesafioBackEnd.Application.User.Login;
using DesafioBackEnd.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackEnd.WebApi.Endpoints;

public class Authentication : CarterModule
{
    public Authentication()
        : base ("/login")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", Login)
            .Produces<JsonResponse<object, object>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<LoginResponse, object>>(StatusCodes.Status200OK)
            .WithOpenApi();
    }

    /// <summary>
    /// Authenticate user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="sender"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>IResult</returns>
    private async Task<IResult> Login(
        [FromBody] LoginRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var token = await sender.Send(
            new LoginCommand(request.Email),
            cancellationToken);

        var response = new JsonResponse<LoginResponse, object>(StatusCodes.Status200OK, token, null);

        return Results.Ok(response);
    }
}
