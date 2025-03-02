using Core.Packages.Application.Common.Behaviors;
using Core.Packages.Application.Common.Services.Auth;
using Core.Packages.Application.Common.Services.Cache;
using Core.Packages.Application.Common.Services.Email;
using Core.Packages.Application.Common.Services.JWT;
using Core.Packages.Infrastructure.Configurations.Email;
using Core.Packages.Infrastructure.Configurations.Token;
using Core.Packages.Infrastructure.Services.Auth;
using Core.Packages.Infrastructure.Services.Cache;
using Core.Packages.Infrastructure.Services.Email;
using Core.Packages.Infrastructure.Services.JWT;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

public static class ServiceRegistration
{
    public static IServiceCollection AddCoreInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<ITokenService, JwtService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IEmailService, EmailService>();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog();
        });

        services.AddJWTSettingsService(configuration);
        services.AddSwaggerServices(configuration);
        services.AddRedisSettingsService(configuration);
        return services;
    }

    public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
                }
            });
        });
        return services;
    }

    public static IServiceCollection AddJWTSettingsService(this IServiceCollection services,IConfiguration configuration)
    {
        var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                    ClockSkew = TimeSpan.Zero,
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[JWT ERROR]: {context.Exception.Message}");
                        Console.ResetColor();
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddRedisSettingsService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("Redis");

        var connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
                                ?? redisConfig["ConnectionString"];

        var instanceName = redisConfig["InstanceName"];
        var useSsl = redisConfig.GetValue<bool>("UseSsl", false);
        var password = redisConfig["Password"];

        //if (!string.IsNullOrEmpty(password))
        //{
        //    connectionString += $",password={password}";
        //}

        if (useSsl)
        {
            connectionString += ",ssl=True,abortConnect=False";
        }

        services.AddSingleton<IRedisCacheService>(new RedisCacheService(connectionString, instanceName));
        services.AddSingleton<IRedisLockService>(new RedisLockService(connectionString));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachePipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LockPipelineBehavior<,>));

        return services;
    }

}