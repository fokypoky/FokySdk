using FokySdk.Types.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.DataAccess
{
    public static class RabbitMq
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqSettings settings, ICollection<Type> consumerTypes)
        {
            services.AddMassTransit(options =>
            {
                foreach (var type in consumerTypes)
                {
                    if (!typeof(IConsumer).IsAssignableFrom(type))
                    {
                        throw new ArgumentException();
                    }

                    options.AddConsumer(type);
                }

                options.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(settings.Host, settings.Port, settings.Vhost, c =>
                    {
                        c.Username(settings.User);
                        c.Password(settings.Password);
                    });


                });
            });

            return services;
        }
    }
}
