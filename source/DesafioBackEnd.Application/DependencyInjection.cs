using AutoMapper;
using DesafioBackEnd.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioBackEnd.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly);
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly);

        var autoMapperConfiguration = new MapperConfiguration(configuration =>
        {
            // add map configuration here
        });

        var mapper = autoMapperConfiguration.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}
