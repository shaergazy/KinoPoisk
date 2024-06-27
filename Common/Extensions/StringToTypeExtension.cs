using System;
using System.Globalization;
using Common.Exceptions;

namespace Common.Extensions;

public static class StringToTypeExtension
{
    public static int? ToNullableInt(this string str, string errorNumber)
    {
        if (string.IsNullOrWhiteSpace(str))
            return null;

        return str.ToInt(errorNumber);
    }

    public static int ToInt(this string str, string errorNumber)
    {
        if (!int.TryParse(str, out var res))
            throw new InnerException($"{errorNumber} Не удалось конвертировать '{str}' в int.");

        return res;
    }

    public static int? ToInt(this string str)
    {
        if (int.TryParse(str, out var iv))
            return iv;

        return null;
    }

    public static decimal? ToNullableDecimal(this string str, string errorNumber)
    {
        if (string.IsNullOrWhiteSpace(str))
            return null;

        return str.ToDecimal(errorNumber);
    }

    public static decimal ToDecimal(this string str, string errorNumber)
    {
        if (string.IsNullOrWhiteSpace(str))
            return decimal.Zero;

        if (decimal.TryParse(str, out var withDot))
            return withDot;

        if (decimal.TryParse(str.Replace('.', ','), out var withComma))
            return withComma;

        throw new InnerException($"{errorNumber} Не удалось конвертировать '{str}' в decimal.");
    }

    public static decimal? ToDecimal(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        if (decimal.TryParse(str, out var withDot))
            return withDot;

        if (decimal.TryParse(str.Replace('.', ','), out var withComma))
            return withComma;

        return null;
    }

    public static DateTime? ToNullableDateTime(this string str, string errorNumber)
    {
        return string.IsNullOrWhiteSpace(str)
            ? (DateTime?)null
            : str.ToDateTime(errorNumber);
    }

    public static DateTime ToDateTime(this string str, string errorNumber)
    {
        if (DateTime.TryParse(str, out var utc))
            return utc;

        if (DateTime.TryParse(str, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out var en))
            return en;

        if (DateTime.TryParse(str, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out var ru))
            return ru;

        throw new InnerException($"{errorNumber} Не удалось конвертировать '{str}' в DateTime.");
    }
}
