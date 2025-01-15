namespace Core.Packages.Security.OAuth.Exceptions;

// OAuth token hatalarını yönetmek için özel exception
public class OAuthTokenException : Exception
{
    public string ErrorCode { get; }

    public OAuthTokenException(string message, string errorCode = "invalid_token") 
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public OAuthTokenException(string message, Exception innerException, string errorCode = "invalid_token") 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
} 