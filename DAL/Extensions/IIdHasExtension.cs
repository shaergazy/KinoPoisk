﻿using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions;

public static class IIdHasExtension
{
    public static async Task<T> ById<T, TKey>(this IQueryable<T> entities, TKey id)
        where T : class, IIdHas<TKey>
        where TKey : IEquatable<TKey>
    {
        return await entities.FirstOrDefaultAsync(x => x.Id.Equals(id))
               ?? throw new Exception($"2550. Сущность '{typeof(T).Name}' с кодом = '{id}' не найдена.");
    }
}
