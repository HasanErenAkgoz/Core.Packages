namespace Core.Packages.Application.CrossCuttingConcerns.Logging;

public interface ILoggerService
{
    void Information(string message);
    void Warning(string message);
    void Error(string message);
    void Debug(string message);
    void Verbose(string message);
    void Fatal(string message);
} 