namespace Core.Packages.Infrastructure.Utilities.Results
{
    public class SuccessResult : Result, Packages.Application.Results.IResult
    {
        public SuccessResult(string message)
            : base(true, message)
        {
        }

        public SuccessResult()
            : base(true)
        {
        }
    }
}