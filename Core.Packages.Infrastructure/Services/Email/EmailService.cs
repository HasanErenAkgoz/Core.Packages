using Core.Packages.Application.Common.Services.Email;
using Core.Packages.Infrastructure.Configurations.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Twilio.TwiML.Messaging;

namespace Core.Packages.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                client.Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                client.EnableSsl = true; 

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail),
                    Subject = subject,
                    Body = body.Replace("\n", "<br>"),
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);
                await client.SendMailAsync(mailMessage);
                return true;
            }
        }
    }
}
