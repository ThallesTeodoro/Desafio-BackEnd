using DesafioBackEnd.Infrastructure.Persistence.Data.Seeds;

namespace DesafioBackEnd.WebApi.Extensions;

public static class SeedExtension
{
    public static void ApplySeeds(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        DatabaseSeed.Run(scope.ServiceProvider);
    }
}
