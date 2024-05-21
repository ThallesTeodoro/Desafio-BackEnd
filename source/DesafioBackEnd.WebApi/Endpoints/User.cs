using Carter;
using DesafioBackEnd.Application.User.GetUser;
using DesafioBackEnd.WebApi.Contracts;
using MediatR;

namespace DesafioBackEnd.WebApi.Endpoints;

public class User : CarterModule
{
    public User()
        : base ("/user")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{userId}", GetUser)
            .RequireAuthorization()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .Produces<JsonResponse<UserResponse, object>>(StatusCodes.Status201Created)
            .WithOpenApi();
    }

    private async Task<IResult> GetUser(Guid userId, ISender sender)
    {
        var result = await sender.Send(new GetUserQuery(userId));

        var response = new JsonResponse<UserResponse, object>(StatusCodes.Status200OK, result, null);

        return Results.Ok(response);
    }
}
