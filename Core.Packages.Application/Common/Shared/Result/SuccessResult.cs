namespace Core.Packages.Application.Shared.Result
{
    public class SuccessResult : Result
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