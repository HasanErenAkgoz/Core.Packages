using Core.Packages.Security.Permissions.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class PermissionServiceRegistration
{
    public static IServiceCollection AddPermissionServices(this IServiceCollection services)
    {
        services.AddScoped<IRolePermissionService, RolePermissionManager>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PermissionBehavior<,>));
        
        return services;
    }
} 