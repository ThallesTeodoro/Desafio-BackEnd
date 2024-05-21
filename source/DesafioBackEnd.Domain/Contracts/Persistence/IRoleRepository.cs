using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Check if role name exists
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    bool RoleNameExists(string roleName);
}
