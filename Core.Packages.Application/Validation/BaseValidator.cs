using FluentValidation;
using FluentValidation.Results;
using Core.Packages.CrossCuttingConcerns.Exceptions.Types;
using ValidationException = Core.Packages.CrossCuttingConcerns.Exceptions.Types.ValidationException;

namespace Core.Packages.Application.Validation;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    public override ValidationResult Validate(ValidationContext<T> context)
    {
        var validationResult = base.Validate(context);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.Select(error => 
                new ValidationExceptionModel 
                { 
                    Property = error.PropertyName,
                    Error = error.ErrorMessage 
                }));
            
        return validationResult;
    }
} 