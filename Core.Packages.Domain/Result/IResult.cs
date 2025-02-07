namespace Core.Packages.Domain.Result
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}