using Azure.Storage.Blobs;
using DesafioBackEnd.Domain.Contracts.Authentication;
using DesafioBackEnd.Domain.Contracts.EventBus;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Contracts.Services;
using DesafioBackEnd.Infrastructure.Authentication;
using DesafioBackEnd.Infrastructure.MessageBroker;
using DesafioBackEnd.Infrastructure.Persistence.Repositories;
using DesafioBackEnd.Infrastructure.Services;
using DesafioBackEnd.Infrastructure.Services.Queue;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Add infrastructure services
    /// </summary>
    /// <param name="services"></param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(bussConfigurator =>
        {
            bussConfigurator.SetKebabCaseEndpointNameFormatter();

            bussConfigurator.AddConsumer<NotificationOrderConsumer>();

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
        services.AddTransient<IDeliverymanRepository, DeliverymanRepository>();
        services.AddTransient<IPlanRepository, PlanRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<INotificationRepository, NotificationRepository>();

        services.AddSingleton<IStorageService, BlobStorage>();
        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage")));

        services.AddTransient<IEventBus, EventBus>();

        return services;
    }
}
