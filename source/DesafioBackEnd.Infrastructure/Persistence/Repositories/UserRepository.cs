using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _dbContext
            .Set<User>()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }

    public User? FindByEmail(string email)
    {
        return _dbContext
            .Set<User>()
            .Where(u => u.Email == email)
            .FirstOrDefault();
    }

    public User? FindByIdWithUserRoles(Guid id)
    {
        return _dbContext
            .Set<User>()
            .Include(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
            .Where(u => u.Id == id)
            .AsSplitQuery()
            .FirstOrDefault();
    }
}
