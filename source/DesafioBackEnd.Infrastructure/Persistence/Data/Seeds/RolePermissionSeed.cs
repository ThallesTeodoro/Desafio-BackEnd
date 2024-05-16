using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Seeds;

public static class RolePermissionSeed
{
    public static void Run(IServiceProvider service)
    {
        IRoleRepository roleRepository = service.GetRequiredService<IRoleRepository>();
        IPermissionRepository permissionRepository = service.GetRequiredService<IPermissionRepository>();
        IRolePermissionRepository rolePermissionRepository = service.GetRequiredService<IRolePermissionRepository>();

        List<Permission> permissions = permissionRepository.All();
        List<Role> roles = roleRepository.All();

        var administrator = roles.First(r => r.Name == RoleEnum.Administrator);
        var deliveryman = roles.First(r => r.Name == RoleEnum.Deliveryman);

        if (administrator is not null && permissions.Count > 0)
        {
            var permissionsIds = permissions
                .Where(p => new List<string>()
                    {
                        PermissionEnum.ManageBikes,
                    }
                    .Contains(p.Name))
                .Select(p => p.Id)
                .ToList();

            rolePermissionRepository.UpdateRolePermissions(administrator.Id, permissionsIds);
        }

        if (deliveryman is not null && permissions.Count > 0)
        {
            var permissionsIds = permissions
                .Where(p => new List<string>()
                    {
                        PermissionEnum.DeliverymanRegister,
                    }
                    .Contains(p.Name))
                .Select(p => p.Id)
                .ToList();

            rolePermissionRepository.UpdateRolePermissions(deliveryman.Id, permissionsIds);
        }
    }
}
