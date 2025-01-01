using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Core.Packages.Application.Security;
using Core.Packages.Application.Security.JWT;
using Core.Packages.Infrastructure.Caching;
using Core.Packages.Infrastructure.Caching.Configuration;
using Core.Packages.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Packages.Application.Security.Hashing;
using Core.Packages.Infrastructure.Security.Hashing;

namespace Core.Packages.Infrastructure.Configuration;

internal static class ServiceRegistration
{
    internal static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1024; // 1GB
            options.CompactionPercentage = 0.2; // %20 compaction when limit reached
            options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
        });
        
        services.AddSingleton<ICacheConfiguration, CacheConfiguration>();
        services.AddSingleton<ICacheManager, MemoryCacheManager>();
        return services;
    }

    internal static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IBlacklistedTokenRepository, BlacklistedTokenRepository>();
        return services.AddTokenServices(configuration);
    }
    
    private static IServiceCollection AddTokenServices(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenOptionsSection = configuration.GetSection("TokenOptions");
        if (!tokenOptionsSection.Exists())
            throw new InvalidOperationException("TokenOptions section is missing in configuration");

        var tokenOptions = new TokenOptions
        {
            Audience = tokenOptionsSection["Audience"] ?? throw new InvalidOperationException("Audience is missing"),
            Issuer = tokenOptionsSection["Issuer"] ?? throw new InvalidOperationException("Issuer is missing"),
            AccessTokenExpiration = int.Parse(tokenOptionsSection["AccessTokenExpiration"] ?? "15"),
            RefreshTokenExpiration = int.Parse(tokenOptionsSection["RefreshTokenExpiration"] ?? "60"),
            SecurityKey = tokenOptionsSection["SecurityKey"] ?? throw new InvalidOperationException("SecurityKey is missing")
        };

        services.AddSingleton(tokenOptions);
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}