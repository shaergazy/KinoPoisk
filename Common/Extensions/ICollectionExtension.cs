using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;
using Common.Interfaces;

namespace Common.Extensions;

public static class ICollectionExtension
{
    public static (IEnumerable<TV> Add, IEnumerable<TV> Del, ICollection<(TV Old, TV New)> Edt)
        Merge<TK, TV>(this ICollection<(TK Key, TV Value)> was, ICollection<(TK Key, TV Value)> become, Func<TV, TV, bool> equal)
    {
        var (add, del, oldDict) = was.Merge(become);
        var edt = become
            .Where(x => oldDict.ContainsKey(x.Key) && !equal(x.Value, oldDict[x.Key]))
            .Select(x => (oldDict[x.Key], x.Value))
            .ToList();

        return (add, del, edt);
    }

    public static (IEnumerable<TV> Add, IEnumerable<TV> Del, IDictionary<TK, TV> OldDict)
        Merge<TK, TV>(this ICollection<(TK Key, TV Value)> was, ICollection<(TK Key, TV Value)> become)
    {
        var oldDict = was.ToDictionary(x => x.Key, x => x.Value);
        var newSet = new HashSet<TK>(become.Select(x => x.Key));

        var add = become
            .Where(x => !oldDict.ContainsKey(x.Key))
            .Select(x => x.Value);
        var del = was
            .Where(x => !newSet.Contains(x.Key))
            .Select(x => x.Value);

        return (add, del, oldDict);
    }

    public static void AddRange<T>(this ICollection<T> lst, IEnumerable<T> add)
    {
        foreach (var x in add)
            lst.Add(x);
    }

    public static void RemoveRange<T>(this ICollection<T> lst, IEnumerable<T> del)
    {
        foreach (var x in del)
            lst.Remove(x);
    }

    public static IDictionary<TK, TV> ToDictionaryOrThrow<TK, TV>(this ICollection<(TK Key, TV Value)> lst, string errorNumber)
    {
        var dict = new Dictionary<TK, TV>();
        foreach (var (key, value) in lst)
        {
            if (dict.ContainsKey(key))
                throw new InnerException($"{errorNumber} Значения в списке не уникальны");

            dict[key] = value;
        }

        return dict;
    }

    public static void CheckUnique<TK, TV>(this ICollection<(TK Key, TV Value)> lst, string errorNumber)
    {
        lst.Select(x => x.Key).ToList().CheckUnique(errorNumber);
    }

    public static void CheckUnique<T>(this ICollection<T> lst, string errorNumber)
    {
        var set = new HashSet<T>(lst);
        if (set.Count != lst.Count)
            throw new InnerException($"{errorNumber} Значения в списке не уникальны");
    }

    public static ICollection<(TK, T)> ToKeySelf<T, TK>(this ICollection<T> lst)
        where T : IIdHas<TK>
    {
        return lst
            .Select(x => (x.Id, x))
            .ToList();
    }
}
