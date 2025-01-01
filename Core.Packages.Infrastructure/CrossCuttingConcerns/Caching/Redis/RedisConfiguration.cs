namespace Core.Packages.Infrastructure.CrossCuttingConcerns.Caching.Redis;

public class RedisConfiguration
{
    public string ConnectionString { get; set; }
    public string InstanceName { get; set; }
    public int DatabaseId { get; set; }
} 