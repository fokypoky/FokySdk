using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.WebApi
{
    public static class WebApi
    {
        public static IServiceCollection AddControllersWithNewtonsoft(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            
            return services;
        }
    }
}