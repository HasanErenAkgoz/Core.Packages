using Core.Packages.Application.Common.Messages;
using Core.Packages.Application.Features.Auth.Register.Commands;
using FluentValidation;

namespace Core.Packages.Application.Features.Roles.Commands.Create
{
    public class CreateRoleValidator : AbstractValidator<RegisterCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(Messages.NotEmpty);
        }
    }
}
