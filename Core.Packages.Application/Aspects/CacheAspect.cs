using Castle.DynamicProxy;
using Core.Packages.Application.Aspects.Autofac;
using Core.Packages.Application.CrossCuttingConcerns;
using Core.Packages.Application.CrossCuttingConcerns.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Packages.Application.Aspects;

public class CacheAspect : MethodInterception
{
    private readonly ICacheManager _cacheManager;
    private readonly int _duration;

    public CacheAspect(int duration = 60)
    {
        _duration = duration;
        _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
    }

    protected override void OnBefore(IInvocation invocation)
    {
        var methodName = string.Format($"{invocation.Method.ReflectedType?.FullName}.{invocation.Method.Name}");
        var arguments = invocation.Arguments.ToList();
        var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
        
        if (_cacheManager.IsAdd(key))
        {
            invocation.ReturnValue = _cacheManager.Get(key);
            return;
        }
        
        invocation.Proceed();
        _cacheManager.Add(key, invocation.ReturnValue, _duration);
    }
} 