using FokySdk.Types.DataAccess;
using FokySdk.Types.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.DataAccess
{
    /// <summary>
    /// Provides extension methods for configuring RabbitMq integration with the application.
    /// This static class includes methods for registering RabbitMq consumers and publishers,
    /// as well as setting up RabbitMq-specific configurations in dependency injection.
    /// </summary>
    public static class RabbitMq
    {
        /// <summary>
        /// Configures RabbitMq integration by adding consumers, publishers, and connection settings
        /// to the dependency injection container using MassTransit.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which RabbitMq services will be added.
        /// </param>
        /// <param name="settings">
        /// The <see cref="RabbitMqSettings"/> containing configuration details such as host, port,
        /// username, password, and virtual host for RabbitMq.
        /// </param>
        /// <param name="consumersRegister">
        /// An optional delegate for registering RabbitMq consumers in the <see cref="IBusRegistrationConfigurator"/>.
        /// Set this parameter to null if no consumers need to be registered.
        /// </param>
        /// <param name="consumersAdd">
        /// An optional delegate for configuring RabbitMq consumers in the <see cref="IRabbitMqBusFactoryConfigurator"/>
        /// using the <see cref="IBusRegistrationContext"/>. Set this parameter to null if no specific configuration is needed.
        /// </param>
        /// <param name="publishersRegister">
        /// An optional delegate for registering RabbitMq publishers in the <see cref="IRabbitMqBusFactoryConfigurator"/>.
        /// Set this parameter to null if no publishers need to be registered.
        /// </param>
        /// <returns>
        /// The updated <see cref="IServiceCollection"/> containing the configured RabbitMq integration.
        /// </returns>
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,
            RabbitMqSettings settings,
            Action<IBusRegistrationConfigurator>? consumersRegister,
            Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? consumersAdd,
            Action<IRabbitMqBusFactoryConfigurator>? publishersRegister)
        {
            services.AddMassTransit(options =>
            {
                consumersRegister?.Invoke(options);
                options.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(settings.Host, settings.Port, settings.Vhost, c =>
                    {
                        c.Username(settings.User);
                        c.Password(settings.Password);
                    });

                    consumersAdd?.Invoke(cfg, context);
                    publishersRegister?.Invoke(cfg);
                });
            });

            return services;
        }

        /// <summary>
        /// Configures and adds a RabbitMq consumer endpoint with specified settings, including queue, exchange, routing key,
        /// and optional retry policies, to the RabbitMq bus factory.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the consumer to be configured, which must implement <see cref="IConsumer"/>.
        /// </typeparam>
        /// <param name="factoryConfigurator">
        /// The <see cref="IRabbitMqBusFactoryConfigurator"/> used to configure RabbitMq settings such as endpoints and consumers.
        /// </param>
        /// <param name="busContext">
        /// The <see cref="IBusRegistrationContext"/> that provides access to registered consumers and additional configuration.
        /// </param>
        /// <param name="consumer">
        /// The <see cref="RabbitMqConsumer"/> containing details about the queue, exchange, and routing key used to bind the consumer.
        /// </param>
        /// <param name="retrySettings">
        /// Optional <see cref="RabbitMqRetrySettings"/> to configure retry policies for the consumer. If null, retry policies are not applied.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the exchange type specified in <see cref="RabbitMqConsumer"/> is unknown or invalid.
        /// </exception>
        public static void AddConsumer<T>(IRabbitMqBusFactoryConfigurator factoryConfigurator,
            IBusRegistrationContext busContext, RabbitMqConsumer consumer, RabbitMqRetrySettings? retrySettings = null)
            where T : class, IConsumer
        {
            factoryConfigurator.ReceiveEndpoint(consumer.Queue, endpoint =>
            {
                endpoint.ConfigureConsumeTopology = false;

                if (retrySettings != null)
                {
                    endpoint.UseMessageRetry(r =>
                    {
                        r.Interval(retrySettings.RetryCount, retrySettings.Interval);
                    });
                }
                
                endpoint.Bind(consumer.Exchange, x =>
                {
                    x.ExchangeType = Enum.GetName(typeof(ExchangeType), consumer.ExchangeType)?.ToLower() ?? throw new ArgumentException($"Unknown exchange type. Value: {consumer.ExchangeType}");
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
                x.ExchangeType = Enum.GetName(typeof(ExchangeType), publisher.ExchangeType)?.ToLower() ?? throw new ArgumentException($"Unknown exchange type. Value: {publisher.ExchangeType}");
            });
        }
    }
}
