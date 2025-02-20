using Core.Packages.Application.Shared.Result;

namespace Core.Packages.Application.Common.BusinessRules
{
    public static class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (var result in logics)
            {
                if (!result.Success)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
