using Carter;
using DesafioBackEnd.Application;
using DesafioBackEnd.Infrastructure;
using DesafioBackEnd.Infrastructure.MessageBroker;
using DesafioBackEnd.Infrastructure.Persistence;
using DesafioBackEnd.WebApi.Extensions;
using DesafioBackEnd.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DesafioBackEnd.WebApi.Options;
using DesafioBackEnd.Domain.Enums;
using DesafioBackEnd.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");

    options.UseNpgsql(connectionString, sqlAction =>
    {
        sqlAction.EnableRetryOnFailure(3);

        sqlAction.CommandTimeout(30);
    });

    options.EnableDetailedErrors(true);

    options.EnableSensitiveDataLogging(true);
});

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(PermissionEnum.ManageBikes, p => p.AddRequirements(new UserPermissionRequirement(PermissionEnum.ManageBikes)));
    o.AddPolicy(PermissionEnum.DeliverymanRegister, p => p.AddRequirements(new UserPermissionRequirement(PermissionEnum.DeliverymanRegister)));
    o.AddPolicy(PermissionEnum.BikeRent, p => p.AddRequirements(new UserPermissionRequirement(PermissionEnum.BikeRent)));
    o.AddPolicy(PermissionEnum.RegisterOrder, p => p.AddRequirements(new UserPermissionRequirement(PermissionEnum.RegisterOrder)));
    o.AddPolicy(PermissionEnum.AcceptOrder, p => p.AddRequirements(new UserPermissionRequirement(PermissionEnum.AcceptOrder)));
    o.AddPolicy(PermissionEnum.MakeOrderDelivery, p => p.AddRequirements(new UserPermissionRequirement(PermissionEnum.MakeOrderDelivery)));
});

builder.Services.AddSingleton<IAuthorizationHandler, UserPermissionAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsDockerContainer())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    app.ApplySeeds();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapCarter();

app.Run();