namespace Core.Packages.Application.Security.Exceptions;

public class TokenValidationException : Exception
{
    public TokenValidationException(string message) : base(message)
    {
    }
} 