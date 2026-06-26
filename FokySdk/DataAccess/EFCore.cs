using FokySdk.Types.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FokySdk.DataAccess
{
    public static class EFCore
    {
        public static IServiceCollection AddEfDbContext<T>(this IServiceCollection services, EfCoreConnectionSettings settings)
            where T : DbContext
        {
            services.AddDbContext<T>(options =>
            {
                options.UseNpgsql(settings.ToConnectionString());
            });
            
            return services;
        }
    }
}