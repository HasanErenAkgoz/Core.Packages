using Core.Packages.Application.Repositories;
using Core.Packages.Persistence.Common;
using Core.Packages.Persistence.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddCorePersistenceServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<>));

        services.AddScoped(typeof(IRepository<,>), typeof(EfRepositoryBase<,,>));
        
        return services;
    }
} 