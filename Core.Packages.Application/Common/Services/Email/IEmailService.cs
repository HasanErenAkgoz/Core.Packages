﻿namespace Core.Packages.Application.Common.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
