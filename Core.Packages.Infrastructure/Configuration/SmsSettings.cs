namespace Core.Packages.Infrastructure.Configuration;

public class SmsSettings
{
    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
    public string SenderId { get; set; }
    public bool UseTestMode { get; set; }
} 