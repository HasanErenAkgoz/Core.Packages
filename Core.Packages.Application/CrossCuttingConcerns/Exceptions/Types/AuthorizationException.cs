namespace Core.Packages.CrossCuttingConcerns.Exceptions.Types;

public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message) { }
} 