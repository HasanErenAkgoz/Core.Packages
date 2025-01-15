using System.Reflection;
using Core.Packages.Application.Interfaces;
using Core.Packages.Application.Services.Auth;
using Core.Packages.Domain.Security.Models.Configurations;
using Core.Packages.Security.Authorization;
using Core.Packages.Security.Authorization.Behaviors;
using Core.Packages.Security.Authorization.Discovery;
using Core.Packages.Security.OAuth.Services;
using Core.Packages.Security.TwoFactorAuth.Models;
using Core.Packages.Security.TwoFactorAuth.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Security.DependencyResolvers;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // Authorization servisleri
        services.AddAuthorization(options =>
        {
            // Her permission için policy oluştur
            foreach (var permission in GetAllPermissions())
            {
                options.AddPolicy(permission, policy =>
                    policy.Requirements.Add(new PermissionRequirement(permission)));
            }
        });

        // Handler ve behavior kayıtları
        services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddHttpContextAccessor();

        // Permission service kaydı
        services.AddScoped<IPermissionService, PermissionService>();

        // OAuth servisleri
        services.Configure<OAuthOptions>(configuration.GetSection("OAuth:Google"));
        services.AddHttpClient<IOAuthService, GoogleOAuthService>(client =>
        {
            client.BaseAddress = new Uri("https://accounts.google.com/");
        });
        services.AddScoped<IOAuthManager, OAuthManager>();

        // 2FA servisleri
        services.Configure<TwoFactorAuthOptions>(configuration.GetSection("TwoFactorAuth"));
        services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();

        services.AddHostedService<PermissionDiscoveryService>();

        return services;
    }

    private static IEnumerable<string> GetAllPermissions()
    {
        var permissions = new List<string>
        {
            "Users.Create",
            "Users.Read", 
            "Users.Update",
            "Users.Delete",
            
            "Roles.Create",
            "Roles.Read",
            "Roles.Update", 
            "Roles.Delete",
            
            "Permissions.Create",
            "Permissions.Read",
            "Permissions.Update",
            "Permissions.Delete",
            
            "Settings.Read",
            "Settings.Update",
            
            "Logs.Read",
            "Logs.Delete"
        };

        return permissions;
    }
} 