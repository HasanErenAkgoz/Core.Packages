namespace Core.Packages.Infrastructure.Configuration;

public class SmsSettings
{
    public string DefaultProvider { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string BaseUrl { get; set; }
    public string SenderName { get; set; }

    public SmsSettings()
    {
        DefaultProvider = string.Empty;
        ApiKey = string.Empty;
        ApiSecret = string.Empty;
        BaseUrl = string.Empty;
        SenderName = string.Empty;
    }
} 