using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public bool PermissionNameExists(string permissionName)
    {
        return _dbContext.Set<Permission>().Any(r => r.Name == permissionName);
    }
}
