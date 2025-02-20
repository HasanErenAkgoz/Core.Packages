using Core.Packages.Application.Common.Messages;
using Core.Packages.Application.Features.Auth.Register.Commands;
using FluentValidation;

namespace Core.Packages.Application.Features.Permission.Commands.Create
{
    public class CreatePermissionValidation : AbstractValidator<RegisterCommand>
    {
        public CreatePermissionValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(Messages.NotEmpty);

        }
    }
}
