using Core.Packages.Application.Common.Services.Email;
using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Core.Packages.Application.Features.Auth.ForgotPassword.Commands
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new ErrorResult("Bu email adresine sahip kullanıcı bulunamadı.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"https://localhost:7216/api/Auth/reset-password?email={request.Email}&token={WebUtility.UrlEncode(token)}";

            var emailSent = await _emailService.SendEmailAsync(request.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>");

            if (!emailSent)
                return new ErrorResult("Şifre sıfırlama emaili gönderilemedi.");

            return new SuccessResult("Şifre sıfırlama bağlantısı email adresinize gönderildi.");
        }
    }

}
