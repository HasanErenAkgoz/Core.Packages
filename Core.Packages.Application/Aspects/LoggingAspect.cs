using Castle.DynamicProxy;
using Core.Packages.Application.Aspects.Autofac;
using Microsoft.Extensions.Logging;

public class LoggingAspect : MethodInterception
{
    private readonly ILogger<LoggingAspect> _logger;

    public LoggingAspect(ILogger<LoggingAspect> logger)
    {
        _logger = logger;
    }

    protected override void OnBefore(IInvocation invocation)
    {
        _logger.LogInformation($"Method {invocation.Method.Name} starting...");
    }

    protected override void OnAfter(IInvocation invocation)
    {
        _logger.LogInformation($"Method {invocation.Method.Name} completed.");
    }

    protected override void OnException(IInvocation invocation, Exception e)
    {
        _logger.LogError(e, $"Error in method {invocation.Method.Name}");
    }
} 