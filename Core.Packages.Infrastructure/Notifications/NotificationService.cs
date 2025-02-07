using Core.Packages.Application.Common.DTOs.Notifications;
using Core.Packages.Application.Common.ToolKit.Email;
using Core.Packages.Application.Common.ToolKit.SMS;
using Core.Packages.Domain.Enums;
using Core.Packages.Domain.ToolKit.Notifications;

namespace Core.Packages.Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public NotificationService(IEmailService emailService, ISmsService smsService)
        {
            _emailService = emailService;
            _smsService = smsService;
        }

        public async Task SendNotificationAsync(string recipient, string message, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Email:
                    var email = new EmailDto
                    {
                        To = recipient,
                        Subject = "Notification",
                        Body = message
                    };
                    await _emailService.SendEmailAsync(email);
                    break;
                case NotificationType.Sms:
                    var smsDto = new SmsDto
                    {
                        PhoneNumber = recipient,
                        Message = message
                    };
                    await _smsService.SendSmsAsync(smsDto);
                    break;
                default:
                    throw new ArgumentException("Invalid notification type");

            }
        }
    }
}
