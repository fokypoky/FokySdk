using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.Telemetry
{
    public static class OpenTelemetry
    {
        public static IServiceCollection AddOtelServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
