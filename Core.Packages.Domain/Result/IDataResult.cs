namespace Core.Packages.Domain.Result
{
    public interface IDataResult<out T> : IResult
    {
        T Data { get; }
    }
}