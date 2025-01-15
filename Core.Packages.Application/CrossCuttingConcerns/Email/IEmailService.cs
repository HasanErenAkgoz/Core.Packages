namespace Core.Packages.Application.CrossCuttingConcerns.Email;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
    Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true);
} 