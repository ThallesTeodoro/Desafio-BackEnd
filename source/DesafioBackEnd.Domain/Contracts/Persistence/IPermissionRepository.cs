using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IPermissionRepository : IRepository<Permission>
{
    /// <summary>
    /// Check if permission name exists
    /// </summary>
    /// <param name="permissionName"></param>
    /// <returns></returns>
    bool PermissionNameExists(string permissionName);
}
