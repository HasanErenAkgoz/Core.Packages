using Core.Packages.Application.Common.DTOs.Notifications;
using Core.Packages.Application.Common.ToolKit.Email;
using Core.Packages.Infrastructure.Configurations.Notifications;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Core.Packages.Infrastructure.Notifications
{
    internal class SendGridEmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;

        public SendGridEmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task SendEmailAsync(EmailDto emailDto)
        {
            try
            {
                var client = new SendGridClient(_emailOptions.ApiKey);
                var from = new EmailAddress(_emailOptions.FromEmail, "Notification Service");
                var to = new EmailAddress(emailDto.To);
                var message = MailHelper.CreateSingleEmail(from, to, emailDto.Subject, emailDto.Body, emailDto.Body);
                var response = await client.SendEmailAsync(message);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"SendGrid hata kodu: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SendGrid email gönderme hatası: {ex.Message}");
            }
        }
    }
}
