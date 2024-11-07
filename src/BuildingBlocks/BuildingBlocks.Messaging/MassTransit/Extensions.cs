using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        var hostAddress = new Uri(configuration["MessageBroker:Host"]!);
        var username = configuration["MessageBroker:UserName"]!;
        var password = configuration["MessageBroker:Password"]!;

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
                config.AddConsumers(assembly);

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(hostAddress, host =>
                {
                    host.Username(username);
                    host.Password(password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
