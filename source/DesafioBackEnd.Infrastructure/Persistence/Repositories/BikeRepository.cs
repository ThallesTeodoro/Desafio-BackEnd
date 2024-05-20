using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Dtos;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
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

    public async Task<bool> BikePlateIsUniqueAsync(string plate, Guid excludeId)
    {
        return !await _dbContext
            .Set<Bike>()
            .AnyAsync(b => b.Id != excludeId && b.Plate.ToUpper() == plate.ToUpper());
    }

    public async Task<Bike?> FindAvailableBikeToRentAsync()
    {
        return await _dbContext
            .Set<Bike>()
            .Include(b => b.Rents)
            .FirstOrDefaultAsync(b => b.Rents == null || !b.Rents.Any(r => r.Status == RentStatusEnum.Leased));
    }

    public async Task<PaginationDto<Bike>> ListPaginatedAsync(int page, int pageSize, string? plate)
    {
        var query = _dbContext
            .Set<Bike>()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(plate))
        {
            query = query.Where(b => b.Plate.ToUpper().Contains(plate.ToUpper()));
        }

        return new PaginationDto<Bike>()
        {
            CurrentPage = page,
            PerPage = pageSize,
            Total = await query.CountAsync(),
            Items = await query
                .OrderBy(b => b.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
        };
    }
}
