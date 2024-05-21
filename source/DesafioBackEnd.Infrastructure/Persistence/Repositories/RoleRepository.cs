using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public bool RoleNameExists(string roleName)
    {
        return _dbContext.Set<Role>().Any(r => r.Name == roleName);
    }
}
