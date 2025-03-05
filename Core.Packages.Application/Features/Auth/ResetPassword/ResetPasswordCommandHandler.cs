using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Application.Features.Auth.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult>
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new ErrorResult("Bu e-posta adresine sahip bir kullanıcı bulunamadı.");

            if (request.NewPassword == request.ConfirmPassword)
            {

                var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
                if (!resetResult.Succeeded)
                    return new ErrorResult(string.Join(", ", resetResult.Errors.Select(e => e.Description)));

                return new SuccessResult("Şifreniz başarıyla güncellendi.");
            }
            return new ErrorResult("Şifreler uyuşmuyor.");

        }
    }
}
