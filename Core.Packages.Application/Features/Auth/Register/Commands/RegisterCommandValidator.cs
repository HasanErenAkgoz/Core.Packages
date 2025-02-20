using Core.Packages.Application.Common.Messages;
using Core.Packages.Application.Features.Auth.Register.Commands;
using FluentValidation;


public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(Messages.NotEmpty);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(Messages.NotEmpty);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(Messages.NotEmpty)
            .EmailAddress()
            .WithMessage(Messages.NotValidEmail);

        RuleFor(x => x.Password)
             .NotEmpty()
             .WithMessage(Messages.NotEmpty)
             .MinimumLength(6)
             .WithMessage(Messages.PasswordLength)
             .Matches(@"[A-Z]")
             .WithMessage(Messages.PasswordUppercase)
             .Matches(@"\d")
             .WithMessage(Messages.PasswordDigit)
             .Matches(@"[\!\@\#\$\%\^\&\*\(\)\,\.\?\:\{\}\|\<\>\-]")
             .WithMessage(Messages.PasswordSpecialCharacter);


        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(Messages.NotEmpty)
            .Equal(x => x.Password)
            .WithMessage(Messages.PasswordsDoNotMatch);
    }
}

