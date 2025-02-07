using Core.Packages.Application.Common.DTOs.Notifications;
using Core.Packages.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Common.ToolKit.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto email);

    }
}
