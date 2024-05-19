using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Seeds;

public static class PlanSeed
{
    public static void Run(IServiceProvider service)
    {
        var planRepository = service.GetRequiredService<IPlanRepository>();

        var count = planRepository.Count();

        if (count == 0)
        {
            var plans = new List<Plan>()
            {
                new Plan()
                {
                    Id = Guid.NewGuid(),
                    Days = 7,
                    Value = 30,
                },
                new Plan()
                {
                    Id = Guid.NewGuid(),
                    Days = 15,
                    Value = 28,
                },
                new Plan()
                {
                    Id = Guid.NewGuid(),
                    Days = 30,
                    Value = 22,
                },
            };

            foreach (var plan in plans)
            {
                planRepository.Add(plan);
            }
        }
    }
}
