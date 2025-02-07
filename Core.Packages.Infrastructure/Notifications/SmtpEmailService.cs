using Core.Packages.Application.Common.DTOs.Notifications;
using Core.Packages.Application.Common.ToolKit.Email;
using Core.Packages.Infrastructure.Configurations.Notifications;
using System.Net;
using System.Net.Mail;

namespace Core.Packages.Infrastructure.Notifications
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;

        public SmtpEmailService(EmailOptions emailOptions)
        {
            _emailOptions = emailOptions;
        }

        public async Task SendEmailAsync(EmailDto emailDto)
        {
            try
            {
                using (var client = new SmtpClient(_emailOptions.SmtpServer, _emailOptions.Port))
                {
                    client.Credentials = new NetworkCredential(_emailOptions.Username, _emailOptions.Password);
                    client.EnableSsl = _emailOptions.UseSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailOptions.FromEmail),
                        Subject = emailDto.Subject,
                        Body = emailDto.Body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(emailDto.To);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SMTP email gönderme hatası: {ex.Message}");
            }
        }
    }
}

