using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace FokySdk.Swagger
{
    public static class Swagger
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string serviceName, string? serviceVersion = "v1")
        {
            services.AddOpenApi();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(serviceVersion, new OpenApiInfo { Title = serviceName, Version = serviceVersion });
            });
            
            return services;
        }

        public static WebApplication AddSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
            return app;
        }
    }
}