using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Domain.UnitOfWork;
using Core.Packages.Infrastructure.Startup.HostedServices;
using Core.Packages.Persistence.Context;
using Core.Packages.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCorePersistenceServices<TContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPermissionRepository, PermissionRepositoriy>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
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


                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();

            });
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(5);
            })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<BaseDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager()
                .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<Role>>();

    
            services.AddDataProtection();
       
            //services.AddHostedService<PermissionInitializerHostedService>();
            return services;
        }
    }
}
