using System.Security;

namespace Core.Packages.Application.Security.Exceptions;

public class TokenSecurityException : SecurityException
{
    public TokenSecurityException(string message) : base(message) { }
} 