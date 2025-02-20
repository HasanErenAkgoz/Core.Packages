namespace Core.Packages.Application.Shared.Result
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, string message)
            : base(data, false, message)
        {
        }

        public ErrorDataResult(T data)
            : base(data, false)
        {
        }

        public ErrorDataResult(string message)
            : base(default, false, message)
        {
        }

        public ErrorDataResult(IEnumerable<FluentValidation.Results.ValidationFailure> errors)
            : base(default, false)
        {
        }
    }
}