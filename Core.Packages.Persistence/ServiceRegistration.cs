using Core.Packages.Application.Common.Services.FileUpload;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Domain.UnitOfWork;
using Core.Packages.Infrastructure.Services.FileUpload;
using Core.Packages.Infrastructure.Startup.HostedServices;
using Core.Packages.Persistence.Context;
using Core.Packages.Persistence.Filters;
using Core.Packages.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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
            services.AddScoped<ITranslationRepository, TranslationRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IUploadedFileRepository, UploadedFileRepository>();
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();

            });
           
            services.AddIdentityCoreService(configuration);
            services.AddSwaggerServices(configuration);
            services.AddHostedService<PermissionInitializerHostedService>();
            services.AddDataProtection();
            return services;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerFileUploadOperationFilter>();
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

        public static IServiceCollection AddIdentityCoreService(this IServiceCollection services, IConfiguration configuration)
        {
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

            return services;
        }

    }
}
