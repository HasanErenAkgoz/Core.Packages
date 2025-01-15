using Core.Packages.Infrastructure.Middleware.IPSecurity;
using Core.Packages.Infrastructure.Middleware.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Core.Packages.Application.CrossCuttingConcerns.Storage;
using Core.Packages.Infrastructure.Storage.AWS;
using Core.Packages.Infrastructure.Storage.Azure;
using Core.Packages.Infrastructure.Storage.Local;

namespace Core.Packages.Infrastructure.DependencyResolvers;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // IP Security yapılandırması
        services.Configure<IPSecurityOptions>(configuration.GetSection("IPSecurity"));

        // Rate Limiting yapılandırması
        var rateLimitingOptions = configuration.GetSection("RateLimiting").Get<RateLimitingOptions>() 
            ?? new RateLimitingOptions();

        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.User?.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = rateLimitingOptions.AutoReplenishment,
                        PermitLimit = rateLimitingOptions.PermitLimit,
                        QueueLimit = rateLimitingOptions.QueueLimit,
                        Window = TimeSpan.FromSeconds(rateLimitingOptions.Window)
                    }));

            // Endpoint bazlı rate limit kuralları
            foreach (var rule in rateLimitingOptions.EndpointRules)
            {
                options.AddPolicy(rule.Key, context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User?.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = rateLimitingOptions.AutoReplenishment,
                            PermitLimit = rule.Value.PermitLimit,
                            QueueLimit = rule.Value.QueueLimit,
                            Window = TimeSpan.FromSeconds(rule.Value.Window)
                        }));
            }

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
            };
        });

        services.AddScoped<IStorageService>(serviceProvider =>
        {
            var storageType = configuration["Storage:Type"]?.ToLowerInvariant();
            
            return storageType switch
            {
                "aws" => new S3StorageService(configuration),
                "azure" => new AzureStorageService(configuration),
                _ => new LocalStorageService(configuration)  // Default olarak Local Storage
            };
        });

        return services;
    }
} 