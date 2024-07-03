using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public static class IFormFileExtension
{
    public static async Task<(Stream Source, string FileName)> ToStream(this IFormFile file)
    {
        if (file is null)
            throw new InnerException("2526. The file is not attached", nameof(file));

        var source = new MemoryStream();
        await file.CopyToAsync(source);

        return (source, file.FileName);
    }
}
