using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;

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
}
