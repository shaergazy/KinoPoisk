using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using Common.Exceptions;
using Common.Helpers;

namespace Common.Extensions;

public static class StringLikePathExtension
{
    public static string SelectSuffixFromFiles(this string path)
    {
        return path.SelectSuffixFrom("Files");
    }

    public static string SelectSuffixFrom(this string path, string keyWord, bool byLast = true)
    {
        var inx = byLast
            ? path.LastIndexOf(keyWord, StringComparison.Ordinal)
            : path.IndexOf(keyWord, StringComparison.Ordinal);

        if (inx < 0)
            throw new InnerException($"2512. Ошибочный файл в базе.");

        return path.Substring(inx).ToLink();
    }

    public static string ToLink(this string path)
    {
        return HttpUtility.UrlPathEncode(path.Replace('\\', '/'));
    }

    public static bool FileIsExist(this string path)
    {
        return File.Exists(path);
    }

    public static (string Name, string Path) FileInfo(this string path)
    {
        return (Path.GetFileName(path), path);
    }

    public static FileStream RecreateFile(this string path)
    {
        path.DeleteFileIfExist();
        path.FileDirectoryName().CreateDirectoryIfNotExist();
        var stream = File.Create(path);
        if (!File.Exists(path))
            throw new InnerException($"2575. Не удалось создать файл.");

        return stream;
    }

    public static void DeleteFileIfExist(this string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public static long Size(this string path)
    {
        if (!File.Exists(path))
            return 0;

        return new FileInfo(path).Length;
    }

    public static DateTime CreationTime(this string path)
    {
        if (!File.Exists(path))
            return DateTime.MinValue;

        return new FileInfo(path).CreationTimeUtc;
    }

    public static byte[] Md5(this string path)
    {
        if (!File.Exists(path))
            throw new InnerException($"5511. Файл по пути = '{path}' не найден.");
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(path);

        return md5.ComputeHash(stream);
    }

    public static string FileName(this string path)
    {
        return !File.Exists(path) ? null : Path.GetFileName(path);
    }

    public static string FileExtension(this string path)
    {
        return Path.GetExtension(path);
    }

    public static string PathFileNameWithoutExtension(this string path)
    {
        return !File.Exists(path) ? null : path.FileNameWithoutExtension();
    }

    public static string FileDirectoryName(this string path)
    {
        return Path.GetDirectoryName(path);
    }

    public static string TryCutAssociationIdSuffix(this string str)
    {
        var re = new Regex(@"^(.*)(_\d+)(.*)$");
        var match = re.Match(str);

        return match.Success
            ? $"{match.Groups[1]}{match.Groups[3]}"
            : str;
    }

    public static string FileNameWithoutExtension(this string fileName)
    {
        return Path.GetFileNameWithoutExtension(fileName);
    }

    public static string Combine(this string path, params string[] suffixes)
    {
        foreach (var x in suffixes)
            path = Path.Combine(path, x);

        return path;
    }

    public static void CheckExcel(this string path)
    {
        if (!path.FileIsExcel())
            throw new InnerException($"2589. Этот файл не является excel-ем.", "File");

        if (path.FileIsExcelXls())
            throw new InnerException($"2590. Принимается только xlsx файл.", "File");
    }

    public static bool FileIsExcel(this string path)
    {
        return path.EndsWith(".xlsx") || path.EndsWith(".xls");
    }

    public static bool FileIsExcelXls(this string path)
    {
        return path.EndsWith(".xls");
    }

    public static bool FileIsExcelXlsx(this string path)
    {
        return path.EndsWith(".xlsx");
    }

    public static bool FileIsDoc(this string path)
    {
        return path.EndsWith(".doc");
    }

    public static bool FileIsDocx(this string path)
    {
        return path.EndsWith(".docx");
    }

    public static bool FileIsDocs(this string path)
    {
        return path.EndsWith(".docs");
    }

    public static bool FileIsPdf(this string path)
    {
        return path.EndsWith(".pdf");
    }

    public static bool CheckAllFormats(this string path)
    {
        bool IsExcelFile = FileIsExcelXls(path) || FileIsExcelXlsx(path);
        bool IsWordFile = FileIsDoc(path) || FileIsDocs(path) || FileIsDocx(path);
        bool IsPdfFile = FileIsPdf(path);
        if (IsExcelFile || IsPdfFile || IsWordFile)
            return true;
        else
            return false;
    }

    public static bool ImageIsJPG(this string path)
    {
        return path.EndsWith(".jpg");
    }

    public static bool ImageIsJPEG(this string path)
    {
        return path.EndsWith(".jpeg");
    }

    public static bool ImageIsPNG(this string path)
    {
        return path.EndsWith(".png");
    }
}
