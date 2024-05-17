using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class BikeRepository : Repository<Bike>, IBikeRepository
{
    public BikeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> BikePlateIsUniqueAsync(string plate)
    {
        return !await _dbContext
            .Set<Bike>()
            .AnyAsync(b => b.Plate.ToUpper() == plate.ToUpper());
    }
}
