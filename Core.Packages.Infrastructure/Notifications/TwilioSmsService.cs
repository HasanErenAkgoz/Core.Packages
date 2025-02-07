using Core.Packages.Application.Common.DTOs.Notifications;
using Core.Packages.Application.Common.ToolKit.SMS;
using Core.Packages.Domain.Result;
using Core.Packages.Infrastructure.Configurations.Notifications;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Core.Packages.Infrastructure.Notifications
{
    public class TwilioSmsService : ISmsService
    {
        private readonly SmsOptions _smsOptions;

        public TwilioSmsService(IOptions<SmsOptions> smsOptions)
        {
            _smsOptions = smsOptions.Value;
            TwilioClient.Init(_smsOptions.AccountSid, _smsOptions.AuthToken);
        }

        public async Task SendSmsAsync(SmsDto smsDto)
        {
            try
            {
                var message = await MessageResource.CreateAsync(
                    body: smsDto.Message,
                    from: new Twilio.Types.PhoneNumber(_smsOptions.FromNumber),
                    to: new Twilio.Types.PhoneNumber(smsDto.PhoneNumber)
                );

                if (message.ErrorCode != null)
                {
                    throw new Exception($"Twilio SMS gönderme hatası: {message.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Twilio SMS gönderme hatası: {ex.Message}");
            }
        }
    }
}
