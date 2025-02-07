using Core.Packages.Application.Common.ToolKit.Email;
using Core.Packages.Infrastructure.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Infrastructure.Factories.Notifications
{
    public class EmailServiceFactory
    {
        public static IEmailService CreateEmailService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var emailProvider = configuration["EmailOptions:Provider"] ?? "SMTP";

            return emailProvider switch
            {
                "SMTP" => serviceProvider.GetRequiredService<SmtpEmailService>(),
                "SendGrid" => serviceProvider.GetRequiredService<SendGridEmailService>(),
                _ => throw new Exception("Geçersiz e-posta sağlayıcısı!"),
            };
        }
    }
}
