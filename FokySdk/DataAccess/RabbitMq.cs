using FokySdk.Types.DataAccess;
using FokySdk.Types.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.DataAccess
{
    public static class RabbitMq
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqSettings settings, ICollection<RabbitMqConsumer> consumers)
        {
            services.AddMassTransit(options =>
            {
                foreach (var consumer in consumers)
                {
                    if (!typeof(IConsumer).IsAssignableFrom(consumer.ConsumerType))
                    {
                        throw new ArgumentException();
                    }

                    options.AddConsumer(consumer.ConsumerType);
                }

                options.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(settings.Host, settings.Port, settings.Vhost, c =>
                    {
                        c.Username(settings.User);
                        c.Password(settings.Password);
                    });

                    foreach (var consumer in consumers)
                    {
                        AddConsumer(cfg, context, consumer);
                    }
                });
            });

            return services;
        }

        public static void AddConsumer(IRabbitMqBusFactoryConfigurator factoryConfigurator, IBusRegistrationContext busContext, RabbitMqConsumer consumer)
        {
            factoryConfigurator.ReceiveEndpoint(consumer.Queue, endpoint =>
            {
                endpoint.ConfigureConsumeTopology = false;
                endpoint.Bind(consumer.Exchange, x =>
                {
                    x.ExchangeType = Enum.GetName(typeof(ExchangeType), consumer.ConsumerType);
                    x.RoutingKey = consumer.RoutingKey;
                });

                endpoint.ConfigureConsumer(busContext, consumer.ConsumerType);
            });
        }
    }
}
