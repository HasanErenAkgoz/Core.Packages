using Core.Packages.Domain.Enums;
using Core.Packages.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Domain.ToolKit.Notifications
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string recipient, string message, NotificationType type);

    }
}
