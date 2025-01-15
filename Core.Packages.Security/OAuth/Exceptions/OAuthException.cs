namespace Core.Packages.Security.OAuth.Exceptions;

public class OAuthException : Exception
{
    public OAuthException()
    {
    }

    public OAuthException(string message)
        : base(message)
    {
    }

    public OAuthException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
} 