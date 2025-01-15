namespace Core.Packages.Application.CrossCuttingConcerns.Sms;

public interface ISmsService
{
    Task SendSmsAsync(string phoneNumber, string message);
    Task SendBulkSmsAsync(List<string> phoneNumbers, string message);
} 