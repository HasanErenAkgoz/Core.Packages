using System.Linq;
using Core.Packages.Application.Requests;

namespace Core.Packages.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PageRequest pageRequest)
    {
        if (pageRequest == null)
            return query;

        return query
            .Skip(pageRequest.PageIndex * pageRequest.PageSize)
            .Take(pageRequest.PageSize);
    }
} 