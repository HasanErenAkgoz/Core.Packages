namespace Core.Packages.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendPasswordResetEmailAsync(string email, string resetToken, string userName);
} 