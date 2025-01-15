using System.Reflection;
using Core.Packages.Application.Pipelines.Caching;
using Core.Packages.Application.Pipelines.Logging;
using Core.Packages.Application.Pipelines.Performance;
using Core.Packages.Application.Pipelines.Validation;
using Core.Packages.Application.Services;
using Core.Packages.Infrastructure.Caching;
using Core.Packages.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        });

        
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddScoped<DistributedCacheService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        
        return services;
    }
} 