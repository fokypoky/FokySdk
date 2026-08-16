using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.WebApi
{
    public static class WebApi
    {
        public static IServiceCollection AddControllersWithNewtonsoft(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Formatting = Constants.Constants.SerializerSettings.Formatting;
                options.SerializerSettings.NullValueHandling = Constants.Constants.SerializerSettings.NullValueHandling;
                options.SerializerSettings.DateFormatString = Constants.Constants.SerializerSettings.DateFormatString;
            });
            
            return services;
        }
    }
}