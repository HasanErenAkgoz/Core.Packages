using Core.Packages.Application.CrossCuttingConcerns.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Core.Packages.Infrastructure.Logging;

public class SerilogService : ILoggerService
{
    private readonly ILogger _logger;

    public SerilogService(string applicationName, string elasticSearchUrl)
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProperty("Application", applicationName)
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", 
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                IndexFormat = $"{applicationName}-logs-{DateTime.UtcNow:yyyy-MM}",
                MinimumLogEventLevel = LogEventLevel.Information,
                BatchPostingLimit = 50,
                Period = TimeSpan.FromSeconds(5),
                BufferBaseFilename = "logs/elastic-buffer",
                BufferFileSizeLimitBytes = 5242880,
                BufferLogShippingInterval = TimeSpan.FromSeconds(5),
                DetectElasticsearchVersion = true
            })
            .CreateLogger();
    }

    public void Information(string message) => _logger.Information(message);
    public void Warning(string message) => _logger.Warning(message);
    public void Error(string message) => _logger.Error(message);
    public void Debug(string message) => _logger.Debug(message);
    public void Verbose(string message) => _logger.Verbose(message);
    public void Fatal(string message) => _logger.Fatal(message);
} 