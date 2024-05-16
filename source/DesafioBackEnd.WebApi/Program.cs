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

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsDockerContainer())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapCarter();

app.Run();