using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Seeds;

public static class RoleSeed
{
    public static void Run(IServiceProvider service)
    {
        IRoleRepository roleRepository = service.GetRequiredService<IRoleRepository>();

        var roleNames = RoleEnum.List();

        foreach (var roleName in roleNames)
        {
            if (!roleRepository.RoleNameExists(roleName))
            {
                roleRepository.Add(new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                });
            }
        }
    }
}
