namespace Core.Utilities.Results
{
    public class ErrorResult : Result, Packages.Application.Results.IResult
    {
        public ErrorResult(string message)
            : base(false, message)
        {
        }

        public ErrorResult()
            : base(false)
        {
        }
    }
}