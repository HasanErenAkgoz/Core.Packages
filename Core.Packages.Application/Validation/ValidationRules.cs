using FluentValidation;

namespace Core.Packages.Application.Validation;

public static class ValidationRules
{
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20)
            .Matches("[A-Z]").WithMessage("Şifre en az 1 büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Şifre en az 1 küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Şifre en az 1 rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az 1 özel karakter içermelidir.");
    }

    public static IRuleBuilderOptions<T, string> Name<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("İsim boş olamaz.")
            .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("İsim en fazla 50 karakter olmalıdır.");
    }

    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Email adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
} 