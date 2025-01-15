namespace Core.Packages.Application.CrossCuttingConcerns.Caching;

public interface ICacheConfiguration
{
    string ConnectionString { get; }
    string InstanceName { get; }
    int AbsoluteExpirationInMinutes { get; }
    int SlidingExpirationInMinutes { get; }
} 