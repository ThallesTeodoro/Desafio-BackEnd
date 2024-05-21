using DesafioBackEnd.Domain.Contracts.Authentication;
using DesafioBackEnd.WebApi.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace DesafioBackEnd.WebApi.Filters;

internal class UserPermissionAuthorizationHandler : AuthorizationHandler<UserPermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public UserPermissionAuthorizationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserPermissionRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == false)
        {
            context.Fail();
        }
        else
        {
            using var scope = _serviceProvider.CreateScope();

            var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            var isAuthorized = jwtProvider.CheckUserPermission(context.User, requirement.Permission);

            if (isAuthorized)
            {
                context.Succeed(requirement);
            }
            else
            {
                var response = new JsonResponse<object, object>(StatusCodes.Status401Unauthorized, null, null);

                context.Fail();
            }
        }

        return Task.CompletedTask;
    }
}