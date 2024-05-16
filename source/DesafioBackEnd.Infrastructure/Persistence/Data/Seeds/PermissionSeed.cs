using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Seeds;

public static class PermissionSeed
{
    public static void Run(IServiceProvider service)
    {
        IPermissionRepository permissionRepository = service.GetRequiredService<IPermissionRepository>();

        var permissionNames = PermissionEnum.List();

        foreach (var permissionName in permissionNames)
        {
            if (!permissionRepository.PermissionNameExists(permissionName))
            {
                permissionRepository.Add(new Permission()
                {
                    Id = Guid.NewGuid(),
                    Name = permissionName,
                });
            }
        }
    }
}
