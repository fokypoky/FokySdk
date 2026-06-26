using FokySdk.Types.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace FokySdk.Swagger
{
    /// <summary>
    /// Provides extension methods to configure and integrate Swagger into an ASP.NET Core application.
    /// </summary>
    public static class Swagger
    {
        /// <summary>
        /// Configures Swagger services and integrates them into the application's service collection.
        /// </summary>
        /// <param name="services">The service collection to which Swagger services are added.</param>
        /// <param name="settings">The settings for configuring Swagger.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, SwaggerSettings settings)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(settings.ServiceVersion, new OpenApiInfo { Title = settings.ServiceName, Version = settings.ServiceVersion });
            });
            
            return services;
        }
        
        /// <summary>
        /// Adds Swagger middleware to the application and configures the Swagger UI.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
        /// <returns>The modified <see cref="WebApplication"/>.</returns>
        public static WebApplication AddSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}