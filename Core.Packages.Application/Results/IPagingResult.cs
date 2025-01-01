using System.Collections.Generic;
using Core.Packages.Application.Results;

namespace Core.Packages.Infrastructure.Utilities.Results
{
    public interface IPagingResult<T> : IResult
    {
        /// <summary>
        /// data list
        /// </summary>
        List<T> Data { get; }

        /// <summary>
        /// total number of records
        /// </summary>
        int TotalItemCount { get; }
    }
}
