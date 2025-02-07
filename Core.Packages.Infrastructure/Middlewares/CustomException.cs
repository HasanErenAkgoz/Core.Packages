namespace Core.Packages.Infrastructure.Middlewares
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }

    // NotFoundException.cs
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message) { }
    }

    // ValidationException.cs
    public class ValidationException : CustomException
    {
        public ValidationException(string message) : base(message) { }
    }
}
