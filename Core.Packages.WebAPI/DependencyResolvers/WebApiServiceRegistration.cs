using Core.Packages.Infrastructure.DependencyResolvers;
using Core.Packages.Security.Authorization.IPSecurity;
using Core.Packages.Security.Authorization.RateLimiting;
using Core.Packages.Security.DependencyResolvers;
using Core.Packages.Security.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Threading.RateLimiting;

namespace Core.Packages.WebAPI.DependencyResolvers;

public static class WebApiServiceRegistration
{
    public static IServiceCollection AddWebApiServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        services.AddControllers();

        // Security servislerini ekle
        services.AddSecurityServices(assemblies);

        // Infrastructure servislerini ekle
        services.AddInfrastructureServices(configuration);

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

        // JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["TokenOptions:Issuer"],
                    ValidAudience = configuration["TokenOptions:Audience"],
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(configuration["TokenOptions:SecurityKey"])
                };
            });

        // CORS yapılandırması
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        // Swagger/OpenAPI with JWT support
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Your API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
} 