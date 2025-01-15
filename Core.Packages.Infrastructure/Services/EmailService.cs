using Core.Packages.Application.CrossCuttingConcerns.Email;
using Core.Packages.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Linq;

namespace Core.Packages.Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var email = new MimeMessage();
        
        email.From.Add(new MailboxAddress(_emailSettings.SenderFullName, _emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(emailMessage.To));
        
        if (emailMessage.CC?.Any() == true)
            email.Cc.AddRange(emailMessage.CC.Select(x => MailboxAddress.Parse(x)));
        
        if (emailMessage.BCC?.Any() == true)
            email.Bcc.AddRange(emailMessage.BCC.Select(x => MailboxAddress.Parse(x)));

        var builder = new BodyBuilder();
        if (emailMessage.IsBodyHtml)
            builder.HtmlBody = emailMessage.Body;
        else
            builder.TextBody = emailMessage.Body;

        if (emailMessage.Attachments?.Any() == true)
        {
            foreach (var attachment in emailMessage.Attachments)
            {
                builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
            }
        }

        email.Subject = emailMessage.Subject;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        
        if (_emailSettings.SslEnabled)
            await smtp.ConnectAsync(_emailSettings.Server, _emailSettings.Port, true);
        else
            await smtp.ConnectAsync(_emailSettings.Server, _emailSettings.Port, false);

        if (_emailSettings.AuthenticationRequired)
            await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        var message = new EmailMessage(to, subject, body, isBodyHtml);
        await SendEmailAsync(message);
    }
} 