namespace Core.Packages.Security.OAuth.Exceptions;

// OAuth validasyon hatalarını yönetmek için özel exception
public class OAuthValidationException : Exception
{
    public string ErrorCode { get; }
    public IDictionary<string, string[]> Errors { get; }

    public OAuthValidationException(string message, IDictionary<string, string[]> errors, string errorCode = "validation_failed") 
        : base(message)
    {
        ErrorCode = errorCode;
        Errors = errors;
    }

    public OAuthValidationException(string message, string errorCode = "validation_failed") 
        : base(message)
    {
        ErrorCode = errorCode;
        Errors = new Dictionary<string, string[]>();
    }
} 