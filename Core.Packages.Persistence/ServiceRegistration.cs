using Core.Packages.Domain.Repositories.NewFolder;
using Core.Packages.Domain.UnitOfWork;
using Core.Packages.Persistence.Repositories.EntitiyFrameworkCore;
using Core.Packages.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Persistence
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// Persistence katmanı için gerekli servisleri ve repository'leri kayıt eder
        /// </summary>
        /// <param name="services">Dependency Injection servisleri</param>
        /// <param name="configuration">Uygulama konfigürasyonu</param>
        /// <typeparam name="TContext">DbContext türü</typeparam>
        public static IServiceCollection AddCorePersistenceServices<TContext>(
            this IServiceCollection services, 
            IConfiguration configuration) 
            where TContext : DbContext
        {
            services.AddScoped(typeof(IEntityRepository<>), typeof(EfEntityRepository<,>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<>));

            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"), 
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        
                        sqlOptions.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                    }
                );

                #if DEBUG
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                #endif
            });

            return services;
        }
    }
}
