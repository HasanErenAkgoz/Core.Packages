using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Core.Packages.Infrastructure.Caching;
using Core.Packages.Infrastructure.Caching.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class CacheRegistration
{
    public static IServiceCollection AddMemoryCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1024;
            options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
        });
        services.AddSingleton<ICacheConfiguration, CacheConfiguration>();
        services.AddSingleton<ICacheManager, MemoryCacheManager>();
        return services;
    }
} 