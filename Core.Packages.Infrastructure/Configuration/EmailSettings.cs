namespace Core.Packages.Infrastructure.Configuration;

public class EmailSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string SenderFullName { get; set; }
    public string SenderEmail { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool AuthenticationRequired { get; set; }
    public bool SslEnabled { get; set; }

    public EmailSettings()
    {
        Server = string.Empty;
        SenderFullName = string.Empty;
        SenderEmail = string.Empty;
        UserName = string.Empty;
        Password = string.Empty;
    }
} 