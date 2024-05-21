using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// Find order by id with relationship
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns>Order | null</returns>
    Task<Order?> FindOrderByIdWithRelationshipAsync(Guid orderId);
}
