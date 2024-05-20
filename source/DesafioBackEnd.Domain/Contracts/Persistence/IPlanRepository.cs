using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IPlanRepository : IRepository<Plan>
{
    /// <summary>
    /// Count plans
    /// </summary>
    /// <returns>int</returns>
    int Count();

    /// <summary>
    /// Get plan by days
    /// </summary>
    /// <param name="days"></param>
    /// <returns>Plan</returns>
    Task<Plan> GetPlanByDaysAsync(int days);
}
