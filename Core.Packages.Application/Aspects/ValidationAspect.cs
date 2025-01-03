using Castle.DynamicProxy;
using Core.Packages.Application.Aspects.Autofac;
using Core.Packages.Application.CrossCuttingConcerns.Validation;
using FluentValidation;

namespace Core.Packages.Application.Aspects;

public class ValidationAspect : MethodInterception
{
    private readonly Type _validatorType;

    public ValidationAspect(Type validatorType)
    {
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
        {
            throw new Exception("Wrong validator type");
        }

        _validatorType = validatorType;
    }

    protected override void OnBefore(IInvocation invocation)
    {
        var validator = (IValidator)Activator.CreateInstance(_validatorType);
        var entityType = _validatorType.BaseType.GetGenericArguments()[0];
        var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
        
        foreach (var entity in entities)
        {
            ValidationTool.Validate(validator, entity);
        }
    }
}