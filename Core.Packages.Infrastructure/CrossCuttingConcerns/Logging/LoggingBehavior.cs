using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Core.Packages.Application.Pipelines.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestGuid = Guid.NewGuid().ToString();

        var requestNameWithGuid = $"{requestName} [{requestGuid}]";

        _logger.LogInformation(
            "Beginning Request {RequestNameWithGuid} {@Request}",
            requestNameWithGuid, JsonSerializer.Serialize(request));

        var response = await next();

        _logger.LogInformation(
            "Completed Request {RequestNameWithGuid} with response {@Response}",
            requestNameWithGuid, JsonSerializer.Serialize(response));

        return response;
    }
} 