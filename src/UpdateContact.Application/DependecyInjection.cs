using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UpdateContact.Application.Common.Behaviors;
using UpdateContact.Application.Common.Messaging;

namespace UpdateContact.Application;

public static class DependecyInjection
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependecyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddSingleton<IEventBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var uri = configuration.GetConnectionString("RabbitMq")!;
            var connectionName = configuration["MessageBroker:ConnectionName"]!;
            return new RabbitMQEventBus(uri, connectionName, logger);
        });

        services.AddHealthChecks()
            //.AddSqlServer(configuration.GetConnectionString("Database")!, name: "SQL Server")
            .AddRabbitMQ(configuration.GetConnectionString("RabbitMQ")!, name: "RabbitMQ");
        return services;
    }
}

