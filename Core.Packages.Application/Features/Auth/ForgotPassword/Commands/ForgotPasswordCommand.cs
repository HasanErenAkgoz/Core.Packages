using Core.Packages.Application.Shared.Result;
using MediatR;

namespace Core.Packages.Application.Features.Auth.ForgotPassword.Commands
{
    public class ForgotPasswordCommand : IRequest<IResult>
    {
        public string Email { get; set; }
    }

}
