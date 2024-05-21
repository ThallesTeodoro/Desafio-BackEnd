using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class DeliverymanRepository : Repository<DeliveryDetail>, IDeliverymanRepository
{
    public DeliverymanRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CnhIsUniqueAsync(string cnh)
    {
        return !await _dbContext
            .Set<DeliveryDetail>()
            .AnyAsync(d => d.Cnh == cnh);
    }

    public async Task<bool> CnpjIsUniqueAsync(string cnpj)
    {
        return !await _dbContext
            .Set<DeliveryDetail>()
            .AnyAsync(d => d.Cnpj == cnpj);
    }

    public async Task<DeliveryDetail?> FindByUserIdAsync(Guid userId)
    {
        return await _dbContext
            .Set<DeliveryDetail>()
            .FirstOrDefaultAsync(d => d.UserId == userId);
    }
}
