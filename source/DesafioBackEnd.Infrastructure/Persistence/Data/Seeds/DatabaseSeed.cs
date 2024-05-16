using DesafioBackEnd.Domain.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure.Persistence.Data.Seeds;

public static class DatabaseSeed
{
    public static void Run(IServiceProvider service)
    {
        IUnitOfWork unitOfWork = service.GetRequiredService<IUnitOfWork>();

        PermissionSeed.Run(service);
        RoleSeed.Run(service);
        unitOfWork.SaveChanges();

        RolePermissionSeed.Run(service);
        unitOfWork.SaveChanges();

        UserSeed.Run(service);
        unitOfWork.SaveChanges();
    }
}
