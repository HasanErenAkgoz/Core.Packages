namespace Core.Packages.Application.Shared.Result
{
    public interface IDataResult<out T> : IResult
    {
        T Data { get; }
    }
}