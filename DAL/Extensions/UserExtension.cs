using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions;

public static class UserExtension
{
    public static async Task<T> ByName<T>(this IQueryable<T> query, string userName)
        where T : User
    {
        return await query.FirstOrDefaultAsync(x => x.UserName == userName)
               ?? throw new InnerException($"2510. Пользователь с именем = '{userName}' не найден.");
    }
}
