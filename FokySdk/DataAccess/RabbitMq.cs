using FokySdk.Types.DataAccess;
using FokySdk.Types.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.DataAccess
{
    public static class RabbitMq
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqSettings settings, ICollection<RabbitMqConsumer> consumers, Action<IRabbitMqBusFactoryConfigurator> publisherRegister)
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

                    publisherRegister.Invoke(cfg);
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

        public static void AddPublisher<T>(IRabbitMqBusFactoryConfigurator factoryConfigurator, RabbitMqPublisher publisher) where T : class
        {
            factoryConfigurator.Message<T>(x =>
            {
                x.SetEntityName(publisher.Exchange);
            });
            factoryConfigurator.Publish<T>(x =>
            {
                x.Durable = publisher.Durable;
                x.ExchangeType = Enum.GetName(typeof(ExchangeType), publisher.ExchangeType);
            });
        }
    }
}
