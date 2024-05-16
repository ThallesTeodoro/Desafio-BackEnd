using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void UpdateRolePermissions(Guid roleId, List<Guid> permissionsIds)
    {
        List<RolePermission> rolePermissions = _dbContext
            .RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToList();

        if (rolePermissions.Count > 0)
        {
            _dbContext.RolePermissions.RemoveRange(rolePermissions);
        }

        foreach (Guid permissionId in permissionsIds)
        {
            _dbContext.RolePermissions.Add(new RolePermission()
            {
                RoleId = roleId,
                PermissionId = permissionId
            });
        }
    }
}
