using System.Diagnostics;
using FokySdk.Types.Settings;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FokySdk.Telemetry
{
    
    public static class OpenTelemetry
    {
        public static class Providers
        {
            public static ActivitySource TraceSource = new ActivitySource(Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? $"Unknown service instance {Guid.NewGuid()}");
        }
        
        public static IServiceCollection AddOtelServices(this IServiceCollection services, OtelServiceInfo serviceInfo, OtelSettings settings)
        {
            Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(
                [
                    new TraceContextPropagator(),
                    new BaggagePropagator()
                ]
            ));
            
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceInfo.ServiceName))
                .WithTracing(tracing =>
                {
                    tracing.AddSource(serviceInfo.ServiceName)
                        .AddSource("MassTransit");

                    if (settings.UseAspNetCoreInstrumentation)
                    {
                        tracing.AddAspNetCoreInstrumentation(options =>
                        {
                            options.RecordException = true;
                        });
                    }

                    if (settings.UseHttpClientInstrumentation)
                    {
                        tracing.AddHttpClientInstrumentation(options =>
                        {
                            options.RecordException = true;
                        });
                    }

                    if (settings.UseEntityFrameworkInstrumentation)
                    {
                        tracing.AddEntityFrameworkCoreInstrumentation();
                    }

                    if (settings.UseMassTransitInstrumentation)
                    {
                        tracing.AddMassTransitInstrumentation();
                    }

                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(serviceInfo.GrpcEndpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    });
                });
            
            

            services.AddSingleton(Providers.TraceSource);
            
            return services;
        }
    }
}
