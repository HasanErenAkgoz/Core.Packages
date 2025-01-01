using Castle.DynamicProxy;

namespace Core.Packages.Application.Aspects.Autofac;

public interface IInterceptor
{
    void Intercept(IInvocation invocation);
} 