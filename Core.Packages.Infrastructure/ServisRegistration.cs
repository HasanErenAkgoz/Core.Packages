using Core.Packages.Application.Common.ToolKit.Email;
using Core.Packages.Application.Common.ToolKit.SMS;
using Core.Packages.Domain.ToolKit.Notifications;
using Core.Packages.Domain.ToolKit.Security;
using Core.Packages.Infrastructure.Configurations.Notifications;
using Core.Packages.Infrastructure.Factories.Notifications;
using Core.Packages.Infrastructure.Notifications;
using Core.Packages.Infrastructure.Redis;
using Core.Packages.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

public static class ServiceRegistration
{
    public static IServiceCollection AddCoreInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Email ve SMS konfigürasyonlarý
        services.Configure<EmailOptions>(configuration.GetSection("EmailOptions"));
        services.Configure<SmsOptions>(configuration.GetSection("SmsOptions"));

        // Redis konfigürasyonu
        var redisConfiguration = configuration.GetSection("Redis:Configuration").Value;
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration;
            options.InstanceName = configuration["Redis:InstanceName"];
        });

        // Logging yapýlandýrmasý
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog();
        });

        // Servis kayýtlarý
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<SmtpEmailService>();
        services.AddTransient<SendGridEmailService>();
        services.AddTransient<TwilioSmsService>();
        services.AddTransient<RedisService>();

        services.AddSingleton<IEmailService>(provider =>
            EmailServiceFactory.CreateEmailService(provider, configuration));

        services.AddSingleton<ISmsService>(provider =>
            SmsServiceFactory.CreateSmsService(provider, configuration));

        return services;
    }
}