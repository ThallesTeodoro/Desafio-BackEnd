using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
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

    public async Task<User?> FindWithRelationshipAsync(Guid id)
    {
        return await _dbContext
            .Set<User>()
            .Include(u => u.Role)
            .Include(u => u.DeliveryDetail)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAvailableDeliverymanAsync()
    {
        return await _dbContext
            .Set<User>()
            .Include(u => u.DeliveryDetail)
            .Include(u => u.Rents)
            .Include(u => u.Orders)
            .AsSingleQuery()
            .Where(u =>
                u.DeliveryDetail != null &&
                u.Rents.Any(r => r.Status == RentStatusEnum.Leased) &&
                !u.Orders.Any(o => o.Status == OrderStatusEnum.Accepted))
            .ToListAsync();
    }
}
