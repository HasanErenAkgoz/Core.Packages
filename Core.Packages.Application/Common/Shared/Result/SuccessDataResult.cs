namespace Core.Packages.Application.Shared.Result
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data, string message)
            : base(data, true, message)
        {
        }

        public SuccessDataResult(T data)
            : base(data, true)
        {
        }

        public SuccessDataResult(string message)
            : base(default, true, message)
        {
        }

        public SuccessDataResult(IEnumerable<Domain.Entities.Permission> enumerable)
            : base(default, true)
        {
        }
    }
}