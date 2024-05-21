using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DesafioBackEnd.Domain.Contracts.Authentication;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DesafioBackEnd.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly IUserRepository _userRepository;

    public JwtProvider(IOptions<JwtOptions> options, IUserRepository userRepository)
    {
        _options = options.Value;
        _userRepository = userRepository;
    }

    public string Generate(User user)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool CheckUserPermission(ClaimsPrincipal user, string permission)
    {
        string userId = user
            .Claims
            .Where(c => c.Type == ClaimTypes.NameIdentifier)
            .First()
            .Value;

        var authUser = _userRepository.FindByIdWithUserRoles(Guid.Parse(userId));

        if (authUser is null)
        {
            return false;
        }

        var userPermissions = authUser
            .Role
            .RolePermissions
            .Select(rp => rp.Permission.Name)
            .ToList();

        return userPermissions.Contains(permission);
    }
}
