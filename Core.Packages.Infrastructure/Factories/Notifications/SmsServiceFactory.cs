using Core.Packages.Application.Common.ToolKit.SMS;
using Core.Packages.Infrastructure.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Infrastructure.Factories.Notifications
{
    public class SmsServiceFactory
    {
        public static ISmsService CreateSmsService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var smsProvider = configuration["SmsOptions:Provider"] ?? "Twilio";

            return smsProvider switch
            {
                "Twilio" => serviceProvider.GetRequiredService<TwilioSmsService>(),
                _ => throw new Exception("Geçersiz SMS sağlayıcısı!"),
            };
        }
    }
}
