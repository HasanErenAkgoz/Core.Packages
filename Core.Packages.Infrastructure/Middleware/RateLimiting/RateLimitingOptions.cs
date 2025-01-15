namespace Core.Packages.Infrastructure.Middleware.RateLimiting;

public class RateLimitingOptions
{
    public bool EnableRateLimiting { get; set; } = true;
    public int PermitLimit { get; set; } = 100;
    public int Window { get; set; } = 60; // seconds
    public int QueueLimit { get; set; } = 2;
    public bool AutoReplenishment { get; set; } = true;
    
    // Özel rate limit kuralları
    public Dictionary<string, EndpointRateLimit> EndpointRules { get; set; } = new();
}

public class EndpointRateLimit
{
    public int PermitLimit { get; set; }
    public int Window { get; set; }
    public int QueueLimit { get; set; }
} 