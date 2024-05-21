using Microsoft.AspNetCore.Authorization;

namespace DesafioBackEnd.WebApi.Filters;

internal class UserPermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; set; }

    public UserPermissionRequirement(string permission) => Permission = permission;
}
