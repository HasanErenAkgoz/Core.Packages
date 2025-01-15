using Core.Utilities.Results;

namespace Core.Packages.Infrastructure.Common.Response;

public class PaginatedResult<T> : DataResult<IList<T>>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PaginatedResult() : base(default, true, string.Empty)
    {
        CurrentPage = 1;
        PageSize = 10;
    }

    private PaginatedResult(IList<T> data, bool success, string message) : base(data, success, message)
    {
        CurrentPage = 1;
        PageSize = 10;
    }

    public static PaginatedResult<T> Success(IList<T> data, int currentPage, int pageSize, int totalCount)
    {
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var result = new PaginatedResult<T>(data, true, string.Empty)
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalCount = totalCount
        };

        return result;
    }

    public static PaginatedResult<T> Fail(string message) =>
        new(default, false, message);
}