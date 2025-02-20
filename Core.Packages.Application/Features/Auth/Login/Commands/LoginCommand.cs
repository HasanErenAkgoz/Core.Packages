using Core.Packages.Application.Common.Services.JWT;
using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Application.Features.Auth.Login.Commands
{
    public class LoginCommand : IRequest<IDataResult<AccessToken>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, IDataResult<AccessToken>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<IDataResult<AccessToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new ErrorDataResult<AccessToken>("User not found or password is incorrect");
            }

            var tokens = await _tokenService.CreateToken<AccessToken>(user);
            return new SuccessDataResult<AccessToken>(new AccessToken
            {
                Token = tokens.Token,
                RefreshToken = tokens.RefreshToken,
                Claims = tokens.Claims,
                Expiration = tokens.Expiration
            });
        }
    }

}
