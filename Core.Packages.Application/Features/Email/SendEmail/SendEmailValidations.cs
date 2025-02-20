using Core.Packages.Application.Common.Messages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Features.Email.SendEmail
{
    public partial class SendEmailValidations : AbstractValidator<SendEmailCommand>
    {
        public SendEmailValidations()
        {
            RuleFor(x => x.To)
               .NotEmpty()
               .WithMessage(Messages.NotEmpty)
               .EmailAddress()
               .WithMessage(Messages.NotValidEmail);

            RuleFor(x => x.Subject).NotEmpty().WithMessage(Messages.NotEmpty);
            RuleFor(x => x.Body).NotEmpty().WithMessage(Messages.NotEmpty);
        }
    }
}
