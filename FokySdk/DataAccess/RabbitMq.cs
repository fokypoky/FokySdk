using FokySdk.Types.DataAccess;
using FokySdk.Types.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.DataAccess
{
    public static class RabbitMq
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqSettings settings, Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext> consumerRegister, Action<IRabbitMqBusFactoryConfigurator> publisherRegister)
        {
            services.AddMassTransit(options =>
            {
                options.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(settings.Host, settings.Port, settings.Vhost, c =>
                    {
                        c.Username(settings.User);
                        c.Password(settings.Password);
                    });

                    consumerRegister.Invoke(cfg, context);
                    publisherRegister.Invoke(cfg);
                });
            });

            return services;
        }

        public static void AddConsumer<T>(IRabbitMqBusFactoryConfigurator factoryConfigurator, IBusRegistrationContext busContext, RabbitMqConsumer consumer) where T : class, IConsumer
        {
            factoryConfigurator.ReceiveEndpoint(consumer.Queue, endpoint =>
            {
                endpoint.ConfigureConsumeTopology = false;
                endpoint.Bind(consumer.Exchange, x =>
                {
                    x.ExchangeType = Enum.GetName(typeof(ExchangeType), consumer.ConsumerType);
                    x.RoutingKey = consumer.RoutingKey;
                });

                endpoint.ConfigureConsumer<T>(busContext);
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
