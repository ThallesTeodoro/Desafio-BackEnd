using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Order?> FindOrderByIdWithRelationshipAsync(Guid orderId)
    {
        return await _dbContext
            .Set<Order>()
            .Include(o => o.Notifications)
                .ThenInclude(n => n.User)
                    .ThenInclude(u => u.DeliveryDetail)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}
