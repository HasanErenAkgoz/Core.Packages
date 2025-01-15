namespace Core.Packages.Security.OAuth.Exceptions;

// OAuth istemci hatalarını yönetmek için özel exception
public class OAuthClientException : Exception
{
    public string ErrorCode { get; }

    public OAuthClientException(string message, string errorCode = "invalid_client") 
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public OAuthClientException(string message, Exception innerException, string errorCode = "invalid_client") 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
} 