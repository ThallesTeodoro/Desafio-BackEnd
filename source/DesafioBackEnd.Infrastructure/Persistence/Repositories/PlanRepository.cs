using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class PlanRepository : Repository<Plan>, IPlanRepository
{
    public PlanRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public int Count()
    {
        return _dbContext.Set<Plan>().Count();
    }

    public async Task<Plan> GetPlanByDaysAsync(int days)
    {
        return await _dbContext
            .Set<Plan>()
            .OrderBy(p => p.Days)
            .FirstAsync(c => days <= c.Days);
    }
}
