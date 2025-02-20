using Core.Packages.Application.Common.Messages;
using Core.Packages.Application.Common.Services.Auth;
using Core.Packages.Application.Common.Services.Email;
using Core.Packages.Application.Shared.Result;
using MediatR;

namespace Core.Packages.Application.Features.Email.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, IResult>
    {
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authenticationService;
        public SendEmailCommandHandler(IEmailService emailService, IAuthenticationService authenticationService)
        {
            _emailService = emailService;
            _authenticationService = authenticationService;
        }

        public async Task<IResult> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
             _authenticationService.EnsurePermissionForHandler<SendEmailCommandHandler>();
            try
            {
                await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
                return new SuccessResult(Messages.SendEmail);
            }
                catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }

}
