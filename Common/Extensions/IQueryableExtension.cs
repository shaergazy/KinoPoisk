using System.Linq;
using Common.Enums;
using Common.Interfaces;

namespace Common.Extensions;

public static class IQueryableExtension
{
    public static IQueryable<T> Page<T>(this IQueryable<T> query, int number, int size)
    {
        return query
            .Skip((number - 1) * size)
            .Take(size);
    }
}
