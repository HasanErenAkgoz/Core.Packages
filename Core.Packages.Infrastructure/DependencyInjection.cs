using Core.Packages.Application.Interfaces;
using Core.Packages.Application.Security.JWT;
using Core.Packages.Infrastructure.Configuration;
using Core.Packages.Infrastructure.Persistence;
using Core.Packages.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISmsService, SmsService>();
        
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<SmsSettings>(configuration.GetSection("SmsSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        
        services.AddHttpClient<SmsService>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        
        return services;
    }
}