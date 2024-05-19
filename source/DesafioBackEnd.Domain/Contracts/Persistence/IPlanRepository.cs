using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IPlanRepository : IRepository<Plan>
{
    /// <summary>
    /// Count plans
    /// </summary>
    /// <returns>int</returns>
    int Count();
}
