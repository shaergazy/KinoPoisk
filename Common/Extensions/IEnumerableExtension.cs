using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;

namespace Common.Extensions;

public static class IEnumerableExtension
{
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> lst)
    {
        return !lst.IsNullOrEmpty();
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> lst)
    {
        return lst.IsNull() || !lst.Any();
    }

    public static T FirstOrThrow<T>(this IEnumerable<T> lst, string errorNumber) where T : class
    {
        return lst.FirstOrDefault() ?? throw new InnerException($"{errorNumber} Список не содержит элемента.");
    }

    public static T FirstOrThrow<T>(this IEnumerable<T> lst, string errorNumber, string modelProperty) where T : class
    {
        return lst.FirstOrDefault() ?? throw new InnerException($"{errorNumber} Список не содержит элемента.", modelProperty);
    }

    public static IEnumerable<T> Page<T>(this IEnumerable<T> lst, int number, int size)
    {
        return lst
            .Skip((number - 1) * size)
            .Take(size);
    }

    public static int TotalPage<T>(this IEnumerable<T> lst, int number, int size)
    {
        var count = lst.Count();
        return count == 0
            ? 1
            : count / size + (count % size > 0 ? 1 : 0);
    }
}
