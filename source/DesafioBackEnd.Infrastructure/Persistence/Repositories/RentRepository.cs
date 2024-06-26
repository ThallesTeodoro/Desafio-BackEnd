using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class RentRepository : Repository<Rent>, IRentRepository
{
    public RentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CheckBikeRentAsync(Guid bikeId)
    {
        return await _dbContext
            .Set<Rent>()
            .AnyAsync(r => r.BikeId == bikeId);
    }

    public async Task<Rent?> FindCurrentUserRentAsync(Guid userId)
    {
        return await _dbContext
            .Set<Rent>()
            .Include(r => r.Plan)
            .Include(r => r.Bike)
            .Include(r => r.User)
                .ThenInclude(r => r.DeliveryDetail)
            .AsSingleQuery()
            .FirstOrDefaultAsync(r => r.UserId == userId && r.Status == RentStatusEnum.Leased);
    }

    public async Task<bool> UserIsAbleToRentAsync(Guid userId)
    {
        return !await _dbContext
            .Set<Rent>()
            .AnyAsync(r => r.UserId == userId && r.Status == RentStatusEnum.Leased);
    }
}
