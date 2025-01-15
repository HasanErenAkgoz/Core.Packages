namespace Core.Packages.Infrastructure.Middleware.IPSecurity;

public class IPSecurityOptions
{
    public bool EnableIPBlocking { get; set; } = true;
    public bool EnableWhitelist { get; set; } = false;
    public List<string> WhitelistedIPs { get; set; } = new();
    public List<string> BlacklistedIPs { get; set; } = new();
    public List<string> WhitelistedIPRanges { get; set; } = new();
    public List<string> BlacklistedIPRanges { get; set; } = new();
} 