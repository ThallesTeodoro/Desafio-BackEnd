using DesafioBackEnd.Domain.Contracts.Authentication;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Infrastructure.Authentication;
using DesafioBackEnd.Infrastructure.MessageBroker;
using DesafioBackEnd.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Add infrastructure services
    /// </summary>
    /// <param name="services"></param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddMassTransit(bussConfigurator =>
        {
            bussConfigurator.SetKebabCaseEndpointNameFormatter();

            bussConfigurator.UsingRabbitMq((context, configurator) =>
            {
                MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IPermissionRepository, PermissionRepository>();
        services.AddTransient<IRolePermissionRepository, RolePermissionRepository>();
        services.AddTransient<IBikeRepository, BikeRepository>();
        services.AddTransient<IRentRepository, RentRepository>();

        return services;
    }
}
