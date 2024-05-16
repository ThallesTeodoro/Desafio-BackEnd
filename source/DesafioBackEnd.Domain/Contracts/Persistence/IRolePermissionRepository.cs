using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IRolePermissionRepository : IRepository<RolePermission>
{
    /// <summary>
    /// Update role permissions
    /// </summary>
    /// <param name="role">The role id</param>
    /// <param name="permissions">List of permissions ids</param>
    void UpdateRolePermissions(Guid roleId, List<Guid> permissionsIds);
}
