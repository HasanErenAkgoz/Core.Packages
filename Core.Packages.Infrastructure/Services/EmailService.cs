using System.Net.Mail;
using Microsoft.Extensions.Options;
using Core.Packages.Infrastructure.Configuration;
using Core.Packages.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Packages.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
            var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
            _logger.LogInformation($"Email sent successfully to {to}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {to}");
            throw;
        }
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetToken, string userName)
    {
        var resetUrl = $"{_emailSettings.PasswordResetUrl}?token={resetToken}";
        var body = $@"
            <h2>Merhaba {userName},</h2>
            <p>Şifrenizi sıfırlamak için aşağıdaki linke tıklayınız:</p>
            <p><a href='{resetUrl}'>Şifremi Sıfırla</a></p>
            <p>Bu link 24 saat geçerlidir.</p>
            <p>Eğer şifre sıfırlama talebinde bulunmadıysanız, bu emaili görmezden gelebilirsiniz.</p>";

        await SendEmailAsync(email, "Şifre Sıfırlama", body);
    }
} 