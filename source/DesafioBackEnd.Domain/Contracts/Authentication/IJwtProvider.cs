using System.Security.Claims;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Authentication;

public interface IJwtProvider
{
    /// <summary>
    /// Generate the jwt token
    /// </summary>
    /// <param name="user"></param>
    /// <returns>The token</returns>
    string Generate(User user);

    /// <summary>
    /// Check if user has permission
    /// </summary>
    /// <param name="user"></param>
    /// <param name="permission"></param>
    /// <returns>true | false</returns>
    bool CheckUserPermission(ClaimsPrincipal user, string permission);
}
